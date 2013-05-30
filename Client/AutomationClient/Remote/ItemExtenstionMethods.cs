using System;
using System.Windows;
using System.Windows.Automation.Peers;
using WindowsPhoneTestFramework.Client.AutomationClient.Helpers;

namespace WindowsPhoneTestFramework.Client.AutomationClient.Remote
{
    public static class ItemExtenstionMethods
    {
        public static FrameworkElement FindSelfOrParentOfType<TElementType>(this FrameworkElement element)
        {
            do
            {
                if (element is TElementType)
                    return element;
                element = element.Parent as FrameworkElement;
            } while (element != null);

            return null;
        }

        public static bool TryGetAutomationPattern<TPatternProvider>(this AutomationPeer automationPeer,
                                                                     PatternInterface patternInterface,
                                                                     out TPatternProvider patternProvider)
            where TPatternProvider : class
        {
            patternProvider = automationPeer.GetPattern(patternInterface) as TPatternProvider;
            return patternProvider != null;
        }

        public static void InvokeAutomationPeer<TTargetType, TPatternProvider>(this FrameworkElement element,
                                                                               PatternInterface patternInterface,
                                                                               Action<TPatternProvider> invokeAction)
            where TPatternProvider : class
        {
            var targetElement = element.FindSelfOrParentOfType<TTargetType>();
            if (targetElement == null)
            {
                throw new TestAutomationException("No appropriate target parent found for " + element.GetType().FullName);
            }

            // automate the click
            var peer = FrameworkElementAutomationPeer.CreatePeerForElement(element);
            if (peer == null)
                throw new TestAutomationException("No automation peer found for " + targetElement.GetType().FullName);

            TPatternProvider patternProvider;
            if (!TryGetAutomationPattern(peer, patternInterface, out patternProvider))
                throw new TestAutomationException("Pattern interface not found for " + targetElement.GetType().FullName);

            invokeAction(patternProvider);
        }
    }
}