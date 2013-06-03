using WindowsPhoneTestFramework.Client.AutomationClient.Remote;

namespace WindowsPhoneTestFramework.Client.AutomationClient.Helpers
{
    public static class AutomationIdentifierExtensions
    {
        public static string ToIdOrName(this AutomationIdentifier automationIdentifier)
        {
            return string.Format("AutomationName: {0}, ElementName : {1}, DisplayText: {2}", 
                                    automationIdentifier.AutomationName, 
                                    automationIdentifier.ElementName, 
                                    automationIdentifier.DisplayedText);
        }
    }
}
