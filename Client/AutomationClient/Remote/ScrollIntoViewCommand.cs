//  ----------------------------------------------------------------------
//  <copyright file="ScrollIntoViewCommand.cs" company="Expensify">
//      (c) Copyright Expensify. http://www.expensify.com
//      This source is subject to the Microsoft Public License (Ms-PL)
//      Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//      All other rights reserved.
//  </copyright>
//  
//  Author - Stuart Lodge, Cirrious. http://www.cirrious.com
//  ------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using WindowsPhoneTestFramework.Client.AutomationClient.Helpers;

namespace WindowsPhoneTestFramework.Client.AutomationClient.Remote
{
    public partial class ScrollIntoViewCommand
    {
        public static readonly List<Func<AutomationPeer, bool>> PatternTesters;
        public static readonly List<Func<UIElement, bool>> UIElementTesters;

        protected override void DoImpl()
        {
            var element = GetUIElement(true);

            if (element == null)
            {
                return;
            }

            // find the parent
            var parent = GetFrameworkElementParent<ScrollViewer>();

            // automate the scroll
            var peer = FrameworkElementAutomationPeer.CreatePeerForElement(parent);
            if (peer == null)
                throw new TestAutomationException("No automation peer found for " + element.GetType().FullName);

            var pattern = peer.GetPattern(PatternInterface.Scroll) as IScrollProvider;
            if (pattern == null)
            {
                throw new TestAutomationException("No scroll pattern found for " + element.GetType().FullName);
            }

            try
            {
                var parentPos = AutomationElementFinder.Position(parent);
                var elementPos = AutomationElementFinder.Position(element as FrameworkElement);
                if (elementPos.Top - parentPos.Top < parentPos.Height)
                {
                    SendSuccessResult();
                    return;
                }

                var scrollHeight = elementPos.Top - (parentPos.Top + (parentPos.Height/2.0));
                var scrollPercentage = (scrollHeight*100.0)/parentPos.Height;
                pattern.SetScrollPercent(ScrollPatternIdentifiers.NoScroll, scrollPercentage);
            }
            catch (Exception exception)
            {
                throw new TestAutomationException("Exception while scrolling item", exception);
            }

            SendSuccessResult();
            return;
        }
    }
}