
using BarRaider.SdTools;
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace ClipboardBuddy
{
    public static class Common
    {
        /*
         * CONSTANTS
         */
        private const double LongPress = 0.5;
        private const double SecurePress = 1.0;
        private const double ClearPress = 2.0;
        
        // Text in Monospaced font in 18pt can fit about 12 chars (lowercase) within the width of the post-it
        private const float LineFontSize = 18.0f;
        private const int LineLength = 12;
        private const int LineNumber = 7;
        
        // Some convenient values to centralize
        private const string secureBackgroundPath = "icons\\postit-secure@2x.png";
        
        /*
         * PUBLIC METHODS
         */

        public static string CoordStringFromKeyCoordinates(KeyCoordinates kc)
        {
            return kc.Row + "x" + kc.Column;
        }

        /// <summary>
        /// This method takes the background ref and the text to render the final image to display.
        /// </summary>
        /// <param name="backFile">The relevant name of the file to use as background</param>
        /// <param name="keyCoords">The coordinates of the key we are dealing with (for the text & color storage)</param>
        /// <returns>The image ready to be sent to the key</returns>
        public static Image RenderKeyImage(string backFile, KeyCoordinates keyCoords = null)
        {
            string coord = keyCoords != null ? CoordStringFromKeyCoordinates(keyCoords) : "";
            string displayText = (string)DataStruct.TextStorageMatrix[coord];

            string backgroundPath = "icons\\" + backFile + "@2x.png";
            
            if (displayText == "")
            {
                // Providing the default image back ;)
                return Image.FromFile("icons\\" + DataStruct.InitialImageNameMatrix[coord] + "@2x.png");
            }
            else
            {
                // Picking a color for the text (without picking twice the same)
                Random rnd = new Random();
                int pick = rnd.Next(DataStruct.UsefulColors.Length);
                Color newColor = DataStruct.UsefulColors[pick];
                
                // Moving away from a property
                while (DataStruct.ColorUsedMatrix.ContainsValue(newColor))
                {
                    pick++;
                    newColor = DataStruct.UsefulColors[pick];
                }
                DataStruct.ColorUsedMatrix[coord] = newColor;
                
                // Other objects for the rendering
                Font myFont = new Font(FontFamily.GenericMonospace, LineFontSize);
                PointF startCoords = new PointF(5.0f, 5.0f);

                // DISPLAY+: show the text rendered multiline as text
                Image[] rendering = GhostGraphicsTools.DrawMultiLinedTextWithBackground(displayText, 0,
                    LineLength, LineNumber, myFont, backgroundPath, newColor, false, startCoords);
                return rendering[0];
            }
        }
        
        /// <summary>
        /// This method takes care of a simple storage/release & can be cleared.
        /// </summary>
        /// <param name="down">the DateTime when the key was pressed</param>
        /// <param name="up">the DateTime when the key was released</param>
        /// <param name="keyCoords">The coordinates of the key we are dealing with (for the text storage)</param>
        /// <returns>The string to use/store/display</returns>
        public static string TwoStateStorage(DateTime down, DateTime up, KeyCoordinates keyCoords)
        {
            TimeSpan pressLength = up - down;
            string coord = CoordStringFromKeyCoordinates(keyCoords);

            if (pressLength.TotalSeconds <= LongPress)
            {
                UpdateClipboard((string)DataStruct.TextStorageMatrix[coord]);
                SendKeys.SendWait("^v");  // careful that using uppercase means activating shift
            }
            else if (pressLength.TotalSeconds >= ClearPress)
            {
                DataStruct.TextStorageMatrix[coord] = "";
            }
            else
            {
                DataStruct.TextStorageMatrix[coord] = ReadClipboard();
            }

            // TO DO: deprecate the return once all type of keys can be rendered as wrapped text
            return (string)DataStruct.TextStorageMatrix[coord];
        }

        /// <summary>
        /// This method takes care of a double clear-or-secure-storage/release & can be cleared.
        /// </summary>
        /// <param name="down">the DateTime when the key was pressed</param>
        /// <param name="up">the DateTime when the key was released</param>
        /// <param name="keyCoords">The coordinates of the key we are dealing with (for the text storage)</param>
        /// <returns>The image to display on the key</returns>
        public static Image ThreeStateStorage(DateTime down, DateTime up, KeyCoordinates keyCoords, string backgroundImg)
        {
            Image finalTile = null;
            
            TimeSpan pressLength = up - down;
            string coord = CoordStringFromKeyCoordinates(keyCoords);

            if (pressLength.TotalSeconds <= LongPress)
            {
                UpdateClipboard((string)DataStruct.TextStorageMatrix[coord]);
                SendKeys.SendWait("^v");  // careful that using uppercase means activating shift
            }
            else if (pressLength.TotalSeconds > SecurePress && pressLength.TotalSeconds < ClearPress)
            {
                DataStruct.TextStorageMatrix[coord] = ReadClipboard();
                // finalTile = Image.FromFile(secureBackgroundPath);
                // TODO: add a smart way to display info about the secure entry to distinguish the key from other secure
                DataStruct.TextStorageMatrix[coord] = "";
                finalTile = RenderKeyImage(secureBackgroundPath, keyCoords);
            }
            else if (pressLength.TotalSeconds >= ClearPress)
            {
                DataStruct.TextStorageMatrix[coord] = "";
                finalTile = Image.FromFile(backgroundImg);
            }
            else // here are are between LongPress & SecurePress
            {
                DataStruct.TextStorageMatrix[coord] = ReadClipboard();
            }
            
            // Simplifying the code by making the tile content the most common: clear with text
            if (finalTile is null)
            {
                finalTile = RenderKeyImage("postit-empty", keyCoords);
            }

            // TO DO: deprecate the return once all type of keys can be rendered as wrapped text
            return finalTile;
        }
        
        /*
         * HELPER METHODS (kept private)
         */
        private static string ReadClipboard()
        {
            string clipSpy = "?";
            
            // Trick from: https://ourcodeworld.com/articles/read/890/how-to-solve-csharp-exception-current-thread-must-be-set-to-single-thread-apartment-sta-mode-before-ole-calls-can-be-made-ensure-that-your-main-function-has-stathreadattribute-marked-on-it
            Thread readT = new Thread((() =>
            {
                clipSpy = Clipboard.GetDataObject() == null ? "Clipboard cannot be accessed ¯\\_(ツ)_/¯"
                                                            : Clipboard.GetDataObject().GetData(DataFormats.Text).ToString();
            }));
            
            // Run your code from a thread that joins the STA Thread
            readT.SetApartmentState(ApartmentState.STA);
            readT.Start();
            readT.Join();

            return clipSpy;
        }

        private static void UpdateClipboard(string newData)
        {
            // Trick from: https://ourcodeworld.com/articles/read/890/how-to-solve-csharp-exception-current-thread-must-be-set-to-single-thread-apartment-sta-mode-before-ole-calls-can-be-made-ensure-that-your-main-function-has-stathreadattribute-marked-on-it
            Thread updateT = new Thread((() =>
            {
                DataObject newDaObj = new DataObject();
                newDaObj.SetData(DataFormats.UnicodeText, true, newData);
                
                // We need to keep it in the clipboard after the thread is gone ;)
                Clipboard.SetDataObject(newDaObj, true);
            }));
            
            // Run your code from a thread that joins the STA Thread
            updateT.SetApartmentState(ApartmentState.STA);
            updateT.Start();
            updateT.Join();
        }
    }
}
