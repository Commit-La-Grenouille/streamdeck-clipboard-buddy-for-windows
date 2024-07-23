
using System;
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
        
        /*
         * PUBLIC METHODS
         */
        
        /* This method takes care of a simple storage/release/clear.
         *
         * param down: the DateTime when the key was pressed
         * param up: the DateTime when the key was released
         * 
         * returns ??
         */
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
                clipSpy = Clipboard.GetDataObject().GetData(DataFormats.Text).ToString();
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