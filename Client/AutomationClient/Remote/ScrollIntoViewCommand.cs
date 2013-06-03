// ----------------------------------------------------------------------
// <copyright file="ScrollIntoViewCommand.cs" company="Nokia">
//     (c) Copyright Nokia. http://www.nokia.com
//     This source is subject to the usual licenses
//     All other rights reserved.
// </copyright>
// 
// Author - Ed Blacker, Sunships Ltd. http://www.sunships.ltd.uk
// ------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
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
                var parentPos = AutomationElementFinder.Position(parent as FrameworkElement);
                var elementPos = AutomationElementFinder.Position(element as FrameworkElement);
                if (elementPos.Top - parentPos.Top < parentPos.Height)
                {
                    SendSuccessResult();
                    return;
                }

                var scrollHeight = elementPos.Top - (parentPos.Top + (parentPos.Height/2.0));
                var scrollPercentage = (scrollHeight * 100.0) / parentPos.Height;
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