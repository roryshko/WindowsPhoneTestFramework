using System.Runtime.Serialization;
using WindowsPhoneTestFramework.Server.WCFHostedAutomationController.Commands;

namespace WindowsPhoneTestFramework.Server.WCFHostedAutomationController
{
    [DataContract]
    public class TestCommand : AutomationElementCommandBase
    {
        [DataMember]
        public string Command { get; set; }

        [DataMember]
        public string Data { get; set; }
    }
}