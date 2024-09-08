using BarRaider.SdTools;
using System;
using System.Drawing;


namespace ClipboardBuddy
{
    [PluginActionId("net.localhost.streamdeck.clipboard-buddy-clear")]
    public class ClipboardBuddyClear : KeypadBase
    {
        /*
         * CONSTRUCTOR
         */
        public ClipboardBuddyClear(SDConnection connection, InitialPayload payload) : base(connection, payload)
        {
            string myCoords = payload.Coordinates.Row + "x" + payload.Coordinates.Column;
            DataStruct.InitialImageNameMatrix[myCoords] = _initialImage;
            DataStruct.ConnectionMatrix[myCoords] = connection;
        }

        
        /*
         * INTERNAL PROPERTIES
         */
        private DateTime _whenPressed;
        private string _initialImage = "postit-unused-clear";

        
        /*
         * BASIC ABSTRACT METHODS SKELETONS
         */
        public override async void Dispose()
        {
            // We need to make sure the last state is a clean display to avoid ghosts when the device powers back on
            Image keyLook = Common.RenderKeyImage(_initialImage);
            await Connection.SetImageAsync(keyLook);
        }

        public override void OnTick()
        {
            Logger.Instance.LogMessage(TracingLevel.DEBUG, "OnTick called");
        }

        public override void ReceivedGlobalSettings(ReceivedGlobalSettingsPayload payload)
        {
            Logger.Instance.LogMessage(TracingLevel.DEBUG, "Global Settings received called");
        }
        
        public override void ReceivedSettings(ReceivedSettingsPayload payload)
        {
            Logger.Instance.LogMessage(TracingLevel.DEBUG, "Settings received called");
        }

        
        /*
         * ACTION CODE
         */
        public override void KeyPressed(KeyPayload payload)
        {
            _whenPressed = DateTime.Now;
        }
        
        public override async void KeyReleased(KeyPayload payload)
        {
            // Processing the behavior of the key to get the relevant text
            Common.TwoStateStorage(_whenPressed, DateTime.Now, payload.Coordinates);

            // DISPLAY+: show the text rendered multiline as text
            Image keyLook = Common.RenderKeyImage("postit-empty", payload.Coordinates);
            await Connection.SetImageAsync(keyLook);
        }
    }
}