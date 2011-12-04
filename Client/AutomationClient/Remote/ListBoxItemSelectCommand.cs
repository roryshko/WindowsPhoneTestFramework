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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;

namespace WindowsPhoneTestFramework.Client.AutomationClient.Remote
{
    public partial class ListBoxItemSelectCommand
    {
        protected override void DoImpl()
        {
            var element = GetFrameworkElementParent<ListBoxItem>();
            if (element == null)
            {
                return;
            }

            element.InvokeAutomationPeer<ListBoxItem, ISelectionItemProvider>(PatternInterface.SelectionItem,
                                                                                 (pattern) =>
                                                                                 {
                                                                                     try
                                                                                     {
                                                                                         pattern.Select();
                                                                                     }
                                                                                     catch (Exception exception)
                                                                                     {
                                                                                         throw new TestAutomationException
                                                                                             ("Exception while invoking list box select pattern",
                                                                                              exception);
                                                                                     }
                                                                                 });
            SendSuccessResult();
        }
    }
}