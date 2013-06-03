// ----------------------------------------------------------------------
// 
// Author - Ed Blacker, Sunships Ltd
// ------------------------------------------------------------------------

using System.Runtime.Serialization;

namespace WindowsPhoneTestFramework.Server.WCFHostedAutomationController.Commands
{
    [DataContract]
    public class SelectorItemCommand : AutomationElementCommandBase
    {
        [DataMember]
        public int IndexOfItemToSelect { get; set; }
    }
}