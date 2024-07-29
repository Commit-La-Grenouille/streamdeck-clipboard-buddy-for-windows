using BarRaider.SdTools;
using System;
using System.Drawing;


namespace ClipboardBuddy
{
    [PluginActionId("net.localhost.streamdeck.clipboard-buddy-secure")]
    public class ClipboardBuddySecure : KeypadBase
    {
        /*
         * CONSTRUCTOR
         */
        public ClipboardBuddySecure(SDConnection connection, InitialPayload payload) : base(connection, payload)
        {
            string myCoords = payload.Coordinates.Row + "x" + payload.Coordinates.Column;
            DataStruct.InitialImageNameMatrix[myCoords] = "postit-unused-secure";
            DataStruct.ConnectionMatrix[myCoords] = connection;
        }

        /*
         * INTERNAL PROPERTIES
         */
        private DateTime _whenPressed;

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
            // Processing the behavior of the key to get the relevant text
            string data = Common.TwoStateStorage(_whenPressed, DateTime.Now, payload.Coordinates);

            // TO DO: move to a rendered image once the PR has been merged (or similar feature is available)
            if (data == "")
            {
                Image keyLook = Image.FromFile("icons\\postit-unused-secure@2x.png");
                await Connection.SetImageAsync(keyLook);
                await Connection.SetTitleAsync("");
            } 
            else
            {
                // DISPLAY BASIC: setting the proper background and showing the timestamp as title
                Image keyLook = Image.FromFile("icons\\postit-secure@2x.png");
                await Connection.SetImageAsync(keyLook);

                string secureText = DateTime.Now.ToShortDateString() + "\n" + DateTime.Now.ToLongTimeString();
                await Connection.SetTitleAsync(secureText);
            }
        }
    }
}