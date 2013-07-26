//  ----------------------------------------------------------------------
//  <copyright file="ScrollCommand.cs" company="Expensify">
//      (c) Copyright Expensify. http://www.expensify.com
//      This source is subject to the Microsoft Public License (Ms-PL)
//      Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//      All other rights reserved.
//  </copyright>
//  
//  Author - Stuart Lodge, Cirrious. http://www.cirrious.com
//  ------------------------------------------------------------------------

using System;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;

namespace WindowsPhoneTestFramework.Client.AutomationClient.Remote
{
    public partial class ScrollCommand
    {
        private ScrollAmount HorizontalScrollAmount
        {
            get { return HintToScrollAmount(HorizontalScroll); }
        }

        private ScrollAmount VerticalScrollAmount
        {
            get { return HintToScrollAmount(VerticalScroll); }
        }

        private static ScrollAmount HintToScrollAmount(int hint)
        {
            switch (hint)
            {
                case -1:
                    return ScrollAmount.SmallDecrement;
                case 0:
                    return ScrollAmount.NoAmount;
                case 1:
                    return ScrollAmount.SmallIncrement;
            }


            if (hint < 0)
                return ScrollAmount.LargeDecrement;

            return ScrollAmount.LargeIncrement;
        }

        protected override void DoImpl()
        {
            var element = GetFrameworkElement();
            if (element == null)
            {
                return;
            }

            element.InvokeAutomationPeer<ScrollViewer, IScrollProvider>(PatternInterface.Scroll,
                                                                        (pattern) =>
                                                                            {
                                                                                try
                                                                                {
                                                                                    pattern.Scroll(
                                                                                        HorizontalScrollAmount,
                                                                                        VerticalScrollAmount);
                                                                                }
                                                                                catch (Exception exception)
                                                                                {
                                                                                    throw new TestAutomationException
                                                                                        ("Exception while invoking list box select pattern",
                                                                                         exception);
                                                                                }
                                                                            });
        }
    }
}