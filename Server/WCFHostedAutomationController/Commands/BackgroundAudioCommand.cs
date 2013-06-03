using System.Runtime.Serialization;

namespace WindowsPhoneTestFramework.Server.WCFHostedAutomationController.Commands
{
    [DataContract]
    public class BackgroundAudioCommand : CommandBase
    {
        [DataMember]
        public AudioInstruction Command { get; set; }
    }

    public enum AudioInstruction
    {
        Stop
    }
}