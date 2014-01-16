using System.Runtime.Serialization;
using WindowsPhoneTestFramework.Server.WCFHostedAutomationController.Commands;

namespace WindowsPhoneTestFramework.Server.WCFHostedAutomationController
{
    [DataContract]
    public class LookForTypeCommand : CommandBase
    {
        [DataMember]
        public string TypeStr { get; set; }
    }
}