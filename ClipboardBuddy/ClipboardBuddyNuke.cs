using BarRaider.SdTools;
using System;
using System.Text;


namespace ClipboardBuddy
{
    [PluginActionId("net.localhost.streamdeck.clipboard-buddy-nuke")]
    public class ClipboardBuddyNuke : KeypadBase
    {
        /*
         * CONSTRUCTOR
         */
        public ClipboardBuddyNuke(SDConnection connection, InitialPayload payload) : base(connection, payload)
        { }

        /*
         * INTERNAL PROPERTIES
         */
        private int PRESS_COUNT = 0;

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
        public override async void KeyPressed(KeyPayload payload)
        {
            PRESS_COUNT += 1;
            if(PRESS_COUNT % 2 == 0) {
                await Connection.SetTitleAsync("KA-BOOM !");
            } else {
                await Connection.SetTitleAsync("POUF !!");
            }
            Logger.Instance.LogMessage(TracingLevel.INFO, "Key Pressed with count" + PRESS_COUNT);
        }
        
        public override void KeyReleased(KeyPayload payload)
        {
            Logger.Instance.LogMessage(TracingLevel.DEBUG, "Key Released");
        }
    }
}