using BarRaider.SdTools;
using System;
using System.Drawing;
using System.Text;
using BarRaider.SdTools.Communication;


namespace ClipboardBuddy
{
    [PluginActionId("net.localhost.streamdeck.clipboard-buddy-line")]
    public class ClipboardBuddyLine : KeypadBase
    {
        /*
         * CONSTRUCTOR
         */
        public ClipboardBuddyLine(SDConnection connection, InitialPayload payload) : base(connection, payload)
        {
            string myCoords = Common.CoordStringFromKeyCoordinates(payload.Coordinates);
            DataStruct.InitialImageNameMatrix[myCoords] = "postit-trashcan";
            DataStruct.ConnectionMatrix[myCoords] = connection;
        }

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
            Logger.Instance.LogMessage(TracingLevel.DEBUG, "Key Pressed");
        }
        
        public override async void KeyReleased(KeyPayload payload)
        {
            for (int col = 0; col < DataStruct.MaxColumnId; col++)
            {
                string thisCoord = payload.Coordinates.Row + "x" + col;
                DataStruct.TextStorageMatrix[thisCoord] = "";

                string imageName = (string)DataStruct.InitialImageNameMatrix[thisCoord];
                if (imageName == "" || imageName.Contains("trashcan") || imageName.Contains("nuke"))
                {
                    continue;  // No need to perform actions on these keys
                }
                
                Image unusedImg = Image.FromFile("icons\\" + DataStruct.InitialImageNameMatrix[thisCoord] + "@2x.png");
                SDConnection targetKeyLink = (SDConnection)DataStruct.ConnectionMatrix[thisCoord];
                
                await targetKeyLink.SetImageAsync(unusedImg);
                await targetKeyLink.SetTitleAsync("");
            }
        }
    }
}