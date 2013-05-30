// ----------------------------------------------------------------------
// 
// Author - Ed Blacker, Sunships Ltd
// ------------------------------------------------------------------------

using System.Runtime.Serialization;

namespace WindowsPhoneTestFramework.Server.WCFHostedAutomationController.Commands
{
    [DataContract]
    public class GetProgressCommand : AutomationElementCommandBase
    {
        [DataMember]
        public bool ReturnEmptyIfNotVisible { get; set; }

        public GetProgressCommand()
        {
            ReturnEmptyIfNotVisible = true; // default is that the command will report {0,0,0,0} for any control with visibility off
        }
    }
}