using BarRaider.SdTools;
using System;


namespace ClipboardBuddy
{
    [PluginActionId("net.localhost.streamdeck.clipboard-buddy-clear")]
    public class ClipboardBuddyClear : KeypadBase
    {
        /*
         * CONSTRUCTOR
         */
        public ClipboardBuddyClear(SDConnection connection, InitialPayload payload) : base(connection, payload)
        { }

        /*
         * INTERNAL PROPERTIES
         */
        private string _data = "";
        private DateTime _whenPressed;
        private DateTime _whenReleased;

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
            _whenReleased = DateTime.Now; 
            _data = Common.TwoStateStorage(_whenPressed, _whenReleased, _data);
            await Connection.SetTitleAsync(_data);
        }
    }
}