
using BarRaider.SdTools;
using System;
using System.Collections;
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
        
        // The enum to select the color from
        // (list: https://learn.microsoft.com/fr-fr/dotnet/api/system.drawing.color?view=net-8.0)
        private static readonly Color[] UsefulColors =
        {
            Color.AliceBlue,
            Color.AntiqueWhite,
            Color.Aqua,
            Color.Aquamarine,
            Color.Azure,
            Color.Beige,
            Color.Bisque,
            //Color.Black,
            Color.BlanchedAlmond,
            //Color.Blue,
            //Color.BlueViolet,
            //Color.Brown,
            Color.BurlyWood,
            Color.CadetBlue,
            Color.Chartreuse,
            //Color.Chocolate,
            Color.Coral,
            Color.CornflowerBlue,
            Color.Cornsilk,
            //Color.Crimson,
            Color.Cyan,
            //Color.DarkBlue,
            //Color.DarkCyan,
            Color.DarkGoldenrod,
            Color.DarkKhaki,
            //Color.DarkMagenta,
            //Color.DarkOliveGreen,
            Color.DarkOrange,
            Color.DarkOrchid,
            //Color.DarkRed,
            Color.DarkSalmon,
            Color.DarkSeaGreen,
            //Color.DarkSlateBlue,
            //Color.DarkSlateGray,
            Color.DarkTurquoise,
            //Color.DarkViolet,
            //Color.DeepPink,
            Color.DeepSkyBlue,
            //Color.DimGray,
            Color.DodgerBlue,
            //Color.Firebrick,
            Color.FloralWhite,
            //Color.ForestGreen,
            //Color.Fuchsia,
            Color.Gainsboro,
            Color.GhostWhite,
            Color.Gold,
            Color.Goldenrod,
            //Color.Gray,
            //Color.Green,
            Color.GreenYellow,
            Color.Honeydew,
            Color.HotPink,
            Color.IndianRed,
            //Color.Indigo,
            Color.Ivory,
            Color.Khaki,
            Color.Lavender,
            Color.LavenderBlush,
            Color.LawnGreen,
            Color.LemonChiffon,
            Color.LightBlue,
            Color.LightCoral,
            Color.LightCyan,
            Color.LightGoldenrodYellow,
            Color.LightGray,
            Color.LightGreen,
            Color.LightPink,
            Color.LightSalmon,
            Color.LightSeaGreen,
            Color.LightSkyBlue,
            //Color.LightSlateGray,
            Color.LightSteelBlue,
            Color.LightYellow,
            Color.Lime,
            Color.LimeGreen,
            Color.Linen,
            Color.Magenta,
            //Color.Maroon,
            Color.MediumAquamarine,
            //Color.MediumBlue,
            Color.MediumOrchid,
            Color.MediumPurple,
            Color.MediumSeaGreen,
            //Color.MediumSlateBlue,
            Color.MediumSpringGreen,
            Color.MediumTurquoise,
            //Color.MediumVioletRed,
            //Color.MidnightBlue,
            Color.MintCream,
            Color.MistyRose,
            Color.Moccasin,
            Color.NavajoWhite,
            //Color.Navy,
            Color.OldLace,
            //Color.Olive,
            Color.OliveDrab,
            Color.Orange,
            //Color.OrangeRed,
            Color.Orchid,
            Color.PaleGoldenrod,
            Color.PaleGreen,
            Color.PaleTurquoise,
            Color.PaleVioletRed,
            Color.PapayaWhip,
            Color.PeachPuff,
            Color.Peru,
            Color.Pink,
            Color.Plum,
            Color.PowderBlue,
            //Color.Purple,
            //Color.Red,
            Color.RosyBrown,
            //Color.RoyalBlue,
            //Color.SaddleBrown,
            Color.Salmon,
            Color.SandyBrown,
            //Color.SeaGreen,
            Color.SeaShell,
            //Color.Sienna,
            Color.Silver,
            Color.SkyBlue,
            //Color.SlateBlue,
            //Color.SlateGray,
            Color.Snow,
            Color.SpringGreen,
            //Color.SteelBlue,
            Color.Tan,
            //Color.Teal,
            Color.Thistle,
            Color.Tomato,
            Color.Turquoise,
            Color.Violet,
            Color.Wheat,
            Color.White,
            Color.WhiteSmoke,
            Color.Yellow,
            Color.YellowGreen
        };

        // To avoid using the same color twice in a row (initialized for a 32 keys Stream Deck to be lazy)
        private static Hashtable _colorUsedMatrix = new Hashtable()
        {
            {"0x0", Color.Transparent},
            {"0x1", Color.Transparent},
            {"0x2", Color.Transparent},
            {"0x3", Color.Transparent},
            {"0x4", Color.Transparent},
            {"0x5", Color.Transparent},
            {"0x6", Color.Transparent},
            {"0x7", Color.Transparent},
            {"1x0", Color.Transparent},
            {"1x1", Color.Transparent},
            {"1x2", Color.Transparent},
            {"1x3", Color.Transparent},
            {"1x4", Color.Transparent},
            {"1x5", Color.Transparent},
            {"1x6", Color.Transparent},
            {"1x7", Color.Transparent},
            {"2x0", Color.Transparent},
            {"2x1", Color.Transparent},
            {"2x2", Color.Transparent},
            {"2x3", Color.Transparent},
            {"2x4", Color.Transparent},
            {"2x5", Color.Transparent},
            {"2x6", Color.Transparent},
            {"2x7", Color.Transparent},
            {"3x0", Color.Transparent},
            {"3x1", Color.Transparent},
            {"3x2", Color.Transparent},
            {"3x3", Color.Transparent},
            {"3x4", Color.Transparent},
            {"3x5", Color.Transparent},
            {"3x6", Color.Transparent},
            {"3x7", Color.Transparent}
        };
        
        
        /*
         * PUBLIC METHODS
         */

        /// <summary>
        /// This method takes the background ref and the text to render the final image to display.
        /// </summary>
        /// <param name="backFile">The relevant name of the file to use as background</param>
        /// <param name="displayText">The text that should be displayed</param>
        /// <param name="typeFile">The relevant name of the file for the key type (to restore the empty display)</param>
        /// <param name="keyCoords">The coordinates of the key we are dealing with (for the color storage)</param>
        /// <returns>The image ready to be send to the key</returns>
        public static Image RenderKeyImage(string backFile, string displayText, string typeFile, KeyCoordinates keyCoords)
        {
            if (displayText == "")
            {
                // Providing the default image back ;)
                return Image.FromFile("icons\\" + typeFile + "@2x.png");
            }
            else
            {
                // Picking a color for the text (without picking twice the same)
                Random rnd = new Random();
                int pick = rnd.Next(UsefulColors.Length);
                Color newColor = UsefulColors[pick];
                
                // Moving away from a property
                while (_colorUsedMatrix.ContainsValue(newColor))
                {
                    pick++;
                    newColor = UsefulColors[pick];
                }
                _colorUsedMatrix[keyCoords.Row + "x" + keyCoords.Column] = newColor;

                // The struct for the final image
                Image[] rendering;
                
                // Other objects for the rendering
                Font myFont = new Font(FontFamily.GenericMonospace, LineFontSize);
                PointF startCoords = new PointF(0.0f, 0.0f);

                // DISPLAY+: show the text rendered multiline as text
                rendering = GraphicsTools.DrawMultiLinedText(displayText, 0, LineLength, LineNumber,
                            myFont, Color.Transparent, newColor, false, startCoords);
                return rendering[0];
            }
        }
        
        /// <summary>
        /// This method takes care of a simple storage/release/clear.
        /// </summary>
        /// <param name="down">the DateTime when the key was pressed</param>
        /// <param name="up">the DateTime when the key was released</param>
        /// <param name="currentData">The current string stored on the key</param>
        /// <returns>The string to use/store/display</returns>
        public static string TwoStateStorage(DateTime down, DateTime up, string currentData)
        {
            TimeSpan pressLength = up - down;
            string resultStr;

            if (pressLength.TotalSeconds <= LongPress)
            {
                UpdateClipboard(currentData);
                SendKeys.SendWait("^v");  // careful that using uppercase means activating shift =))
                resultStr = currentData;
            }
            else if (pressLength.TotalSeconds >= ClearPress)
            {
                resultStr = "";
            }
            else
            {
                resultStr = ReadClipboard();
            }

            return resultStr;
        }
        
        
        /*
         * HELPER METHODS (kept private)
         */
        private static string ReadClipboard()
        {
            string clipSpy = "?";
            
            // Trick from: https://ourcodeworld.com/articles/read/890/how-to-solve-csharp-exception-current-thread-must-be-set-to-single-thread-apartment-sta-mode-before-ole-calls-can-be-made-ensure-that-your-main-function-has-stathreadattribute-marked-on-it
            Thread readT = new Thread((() => {
                if (Clipboard.GetDataObject() == null)
                {
                    clipSpy = "Clipboard cannot be accessed ¯\\_(ツ)_/¯";
                }
                else
                {
                    clipSpy = Clipboard.GetDataObject().GetData(DataFormats.Text).ToString();
                }
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
