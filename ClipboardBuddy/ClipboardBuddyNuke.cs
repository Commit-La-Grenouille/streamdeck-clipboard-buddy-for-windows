using BarRaider.SdTools;
using System;
using System.Text;


namespace ClipboardBuddy
{
    [PluginActionId("net.localhost.streamdeck.clipboard-buddy-for-windows-nuke")]
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
            Logger.Instance.LogMessage(TracingLevel.INFO, "Key Pressed");
        }
        
        public override async void KeyReleased(KeyPayload payload)
        {
            Logger.Instance.LogMessage(TracingLevel.DEBUG, "Key Released");
            await Connection.SetTitleAsync("");
        }
    }
}