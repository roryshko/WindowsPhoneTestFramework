using System.Windows;

namespace WindowsPhoneTestFramework.Client.AutomationClient.Helpers
{
    public interface IValueManipulator
    {
        bool TryGetValue(UIElement element, out string value);
        bool TrySetValue(UIElement element, string value);
    }
}