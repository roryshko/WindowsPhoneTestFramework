// ----------------------------------------------------------------------
// <copyright file="InvokeControlTapActionCommand.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using System;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using WindowsPhoneTestFramework.Client.AutomationClient.Helpers;

namespace WindowsPhoneTestFramework.Client.AutomationClient.Remote
{
    public partial class SelectorItemCommand
    {
        protected override void DoImpl()
        {
            var element = GetFrameworkElement();
            var selector = element as Selector;
            if (selector == null)
            {
                return;
            }

            var item = selector.Items[IndexOfItemToSelect];

            var identifier = new AutomationIdentifier {DisplayedText = item.ToString()};
            var listBoxItem = AutomationElementFinder.FindElement(identifier);
            if (listBoxItem == null)
            {
                SendNotFoundResult(string.Format("SelectorItemCommand: Could not find the list box item element : {0}",
                                                 identifier.ToIdOrName()));
                return;
            }

            var button = AutomationElementFinder.FindElementsChildByType<Button>(listBoxItem);
            if (button != null)  // Workaround for nasty lists that use buttons instead of the list select method
            {
                // automate the invoke
                var buttonPeer = FrameworkElementAutomationPeer.CreatePeerForElement(button);
                var pattern = buttonPeer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                if (pattern == null)
                {
                    SendNotFoundResult(string.Format("SelectorItemCommand: Could not find the invoke pattern : {0}",
                                                     identifier.ToIdOrName()));
                    return;
                }

                try
                {
                    pattern.Invoke();
                }
                catch (Exception exception)
                {
                    SendExceptionFailedResult(exception);
                }

            }
            else
            {

                // automate the select
                var listBoxPeer = FrameworkElementAutomationPeer.CreatePeerForElement(listBoxItem);
                var pattern = listBoxPeer.GetPattern(PatternInterface.SelectionItem) as ISelectionItemProvider;
                if (pattern == null)
                {
                    SendNotFoundResult(string.Format("SelectorItemCommand: Could not find the select pattern : {0}",
                                                     identifier.ToIdOrName()));
                    return;
                }

                try
                {
                    pattern.Select();
                }
                catch (Exception exception)
                {
                    SendExceptionFailedResult(exception);
                }
            }
            SendSuccessResult();
        }
    }
}