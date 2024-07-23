
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
        public const double LongPress = 0.5;
        public const double SecurePress = 1.0;
        public const double ClearPress = 2.0;
        
        // Text in Andale Mono 14 about 16 chars (lowercase) to fit within the width of the post-it
        public const int LineLength = 16;
        
        
        /*
         * PUBLIC METHODS
         */

        /// <summary>
        /// This method takes the background ref and the text to render the final image to display.
        /// </summary>
        /// <param name="backFile">The relevant name of the file to use as background</param>
        /// <param name="displayText">The text that should be displayed</param>
        /// <param name="typeFile">The relevant name of the file for the key type (to restore the empty display)</param>
        /// <returns>The image ready to be send to the key</returns>
        public static Image RenderKeyImage(string backFile, string displayText, string typeFile)
        {
            if (displayText == "")
            {
                // Providing the default image back ;)
                return Image.FromFile("icons\\" + typeFile + "@2x.png");
            }
            else
            {
                // DISPLAY+: show the text rendered multiline as text
                Image[] rendering = GraphicsTools.DrawMultiLinedText(displayText, 0, 12, 7,
                    new Font(FontFamily.GenericMonospace, 18.0f), Color.Transparent, Color.Aqua, false,
                    new PointF(0.0f, 0.0f));
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
