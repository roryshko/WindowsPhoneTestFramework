// 
// Author - Ed Blacker, Sunships Ltd.
// ----------------------------------

using System.Runtime.Serialization;

namespace WindowsPhoneTestFramework.Server.WCFHostedAutomationController.Results
{
    [DataContract]
    public class ProgressResult : ResultBase
    {
        [DataMember]
        public double Min { get; set; }
        [DataMember]
        public double Max { get; set; }
        [DataMember]
        public double Current { get; set; }
    }
}