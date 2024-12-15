using BarRaider.SdTools;
using System;
using System.Drawing;


namespace ClipboardBuddy
{
    [PluginActionId("net.localhost.streamdeck.clipboard-buddy-for-windows-multi")]
    public class ClipboardBuddyDual : KeypadBase
    {
        /*
         * CONSTRUCTOR
         */
        public ClipboardBuddyDual(SDConnection connection, InitialPayload payload) : base(connection, payload)
        {
            string myCoords = Common.CoordStringFromKeyCoordinates(payload.Coordinates);
            DataStruct.InitialImageNameMatrix[myCoords] = _initialImage;
            DataStruct.ConnectionMatrix[myCoords] = connection;
        }
        /*
         * INTERNAL PROPERTIES
         */
        private DateTime _whenPressed;
        private string _initialImage = "postit-unused";

        /*
         * BASIC ABSTRACT METHODS SKELETONS
         */
        public override void Dispose()
        {
            Logger.Instance.LogMessage(TracingLevel.DEBUG, "Destructor called");
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
            // Processing the behavior of the key to get the relevant image to display
            Image keyLook = Common.ThreeStateStorage(_whenPressed, DateTime.Now, payload.Coordinates, _initialImage);
            await Connection.SetImageAsync(keyLook);
        }
    }
}