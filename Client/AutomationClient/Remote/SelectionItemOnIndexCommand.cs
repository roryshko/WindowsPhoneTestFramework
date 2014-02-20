using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Microsoft.Phone.Controls;
using WindowsPhoneTestFramework.Client.AutomationClient.Helpers;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;

namespace WindowsPhoneTestFramework.Client.AutomationClient.Remote
{
    public partial class SelectionItemOnIndexCommand
    {
        protected override void DoImpl()
        {
            var element = GetFrameworkElement();
            var selector = element as ItemsControl;
            if (selector == null)
            {
                return;
            }
            var peer = FrameworkElementAutomationPeer.CreatePeerForElement(element);
            if (peer == null)
            {
                throw new TestAutomationException("No automation peer found for " + element.GetType().FullName);
            }

            if (TryValuePatternAutomation(peer, IndexOfItemToSelect))
            {
                SendSuccessResult();
                return;
            }

            throw new TestAutomationException("No invoke pattern found for " + element.GetType().FullName);
        }

        private static bool TryValuePatternAutomation(AutomationPeer peer, int index)
        {
            var pattern = peer.GetPattern(PatternInterface.Value) as IValueProvider;
            if (pattern == null)
            {
                return false;
            }

            try
            {
                pattern.SetValue(index.ToString());
            }
            catch (Exception exception)
            {
                throw new TestAutomationException("Exception while invoking pattern", exception);
            }

            return true;
        }
    }
}
