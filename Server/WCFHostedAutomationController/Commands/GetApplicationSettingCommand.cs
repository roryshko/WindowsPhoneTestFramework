using System.Runtime.Serialization;

namespace WindowsPhoneTestFramework.Server.WCFHostedAutomationController.Commands
{
    [DataContract]
    public class GetApplicationSettingCommand : AutomationElementCommandBase
    {
        [DataMember]
        public string Key { get; set; }
    }
}