using BarRaider.SdTools;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using BarRaider.SdTools.Wrappers;


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
            _data = Common.TwoStateStorage(_whenPressed, DateTime.Now, _data);
            
            // DISPLAY BASIC: display the text as title
            // await Connection.SetTitleAsync(_data);
            
            // DISPLAY+: show the text rendered multiline as text
            Image keyLook = Common.RenderKeyImage("postit-empty", _data, "postit-unused-clear");
            await Connection.SetImageAsync(keyLook);
        }
    }
}