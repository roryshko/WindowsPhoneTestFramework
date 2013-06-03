using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WindowsPhoneTestFramework.Server.WCFHostedAutomationController.Commands
{
    [DataContract]
    public class SetApplicationSettingsCommand : AutomationElementCommandBase
    {
        [DataMember]
        public Dictionary<string, string> Settings { get; set; }
    }
}