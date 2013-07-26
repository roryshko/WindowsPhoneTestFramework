//  ----------------------------------------------------------------------
//  <copyright file="AutomationPeerCreator.cs" company="Expensify">
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
using System.Windows.Automation.Peers;
using System.Windows.Controls;

namespace WindowsPhoneTestFramework.Client.AutomationClient.Helpers
{
    public static class AutomationPeerCreator
    {
        private static readonly Dictionary<Type, Func<UIElement, AutomationPeer>> Lookups =
            new Dictionary<Type, Func<UIElement, AutomationPeer>>();

        static AutomationPeerCreator()
        {
#warning Many more peers could be added?
            Lookups.Add(typeof (Button), (element) => new ButtonAutomationPeer((Button) element));
            Lookups.Add(typeof (CheckBox), (element) => new CheckBoxAutomationPeer((CheckBox) element));
            Lookups.Add(typeof (HyperlinkButton),
                        (element) => new HyperlinkButtonAutomationPeer((HyperlinkButton) element));
            Lookups.Add(typeof (Image), (element) => new ImageAutomationPeer((Image) element));
            Lookups.Add(typeof (ListBox), (element) => new ListBoxAutomationPeer((ListBox) element));
            Lookups.Add(typeof (ListBoxItem), (element) => new ListBoxItemAutomationPeer((ListBoxItem) element));
            Lookups.Add(typeof (PasswordBox), (element) => new PasswordBoxAutomationPeer((PasswordBox) element));
            Lookups.Add(typeof (ProgressBar), (element) => new ProgressBarAutomationPeer((ProgressBar) element));
            Lookups.Add(typeof (RadioButton), (element) => new RadioButtonAutomationPeer((RadioButton) element));
            Lookups.Add(typeof (Slider), (element) => new SliderAutomationPeer((Slider) element));
            Lookups.Add(typeof (ScrollViewer), (element) => new ScrollViewerAutomationPeer((ScrollViewer) element));
            Lookups.Add(typeof (TextBlock), (element) => new TextBlockAutomationPeer((TextBlock) element));
            Lookups.Add(typeof (TextBox), (element) => new TextBoxAutomationPeer((TextBox) element));
        }

        public static void AddPeerFactory(Type t, Func<UIElement, AutomationPeer> factory)
        {
            lock (Lookups)
            {
                Lookups[t] = factory;
            }
        }

        public static AutomationPeer GetPeer(UIElement element)
        {
            if (element == null)
                return null;

            Func<UIElement, AutomationPeer> func;
            lock (Lookups)
            {
                if (!Lookups.TryGetValue(element.GetType(), out func))
                    return null;
            }

            if (func == null)
                return null;

            return func(element);
        }
    }
}