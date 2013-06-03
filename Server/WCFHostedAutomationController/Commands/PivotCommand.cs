namespace WindowsPhoneTestFramework.Server.WCFHostedAutomationController.Commands
{
    using System.Runtime.Serialization;

    [DataContract]
    public class PivotCommand : AutomationElementCommandBase
    {
        [DataMember]
        public string PivotName { get; set; }

        [DataMember]
        public bool PivotNext { get; set; }

        [DataMember]
        public bool PivotLast { get; set; }
    }
}