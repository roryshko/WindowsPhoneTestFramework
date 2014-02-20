using System.Runtime.Serialization;
using WindowsPhoneTestFramework.Server.WCFHostedAutomationController.Commands;

namespace WindowsPhoneTestFramework.Server.WCFHostedAutomationController
{
    [DataContract]
    public class SelectionItemOnIndexCommand : AutomationElementCommandBase
    {
        [DataMember]
        public int IndexOfItemToSelect { get; set; }
    }
}
