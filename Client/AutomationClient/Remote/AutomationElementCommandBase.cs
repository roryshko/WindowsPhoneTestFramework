// ----------------------------------------------------------------------
// <copyright file="AutomationElementCommandBase.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Controls;
using WindowsPhoneTestFramework.Client.AutomationClient.Helpers;

namespace WindowsPhoneTestFramework.Client.AutomationClient.Remote
{
    public partial class AutomationElementCommandBase
    {
        protected bool AutomationIdIsEmpty
        {
            get
            {
                return AutomationIdentifier == null ||
                       (string.IsNullOrEmpty(AutomationIdentifier.AutomationName)
                       && string.IsNullOrEmpty(AutomationIdentifier.ElementName)
                       && string.IsNullOrEmpty(AutomationIdentifier.DisplayedText));
            }
        }

        protected UIElement GetUIElement(bool sendNotFoundResultOnFail = true)
        {
            var element = AutomationElementFinder.FindElement(AutomationIdentifier, Ordinal, ParentIdentifier);
            if (element == null)
            {
                if (sendNotFoundResultOnFail)
                    SendNotFoundResult(string.Format("GetUIElement: Could not find - {0}", AutomationIdentifier.ToIdOrName()));
                return null;
            }

            return element;
        }

        protected FrameworkElement GetFrameworkElement(bool sendNotFoundResultOnFail = true)
        {
            var element = AutomationElementFinder.FindElement(AutomationIdentifier, Ordinal, ParentIdentifier) as FrameworkElement;
            if (element == null)
            {
                if (sendNotFoundResultOnFail)
                    SendNotFoundResult(string.Format("GetFrameworkElement: Could not find - {0}", AutomationIdentifier.ToIdOrName()));
                return null;
            }

            return element;
        }

        protected FrameworkElement GetFrameworkElementParent<TParentType>(bool sendNotFoundResultOnFail = true)
            where TParentType : FrameworkElement
        {
            var element = AutomationElementFinder.FindElementsNearestParentOfType<TParentType>(AutomationElementFinder.GetRootVisual(), AutomationIdentifier) as FrameworkElement;
            if (element == null)
            {
                if (sendNotFoundResultOnFail)
                    SendNotFoundResult(string.Format("GetFrameworkElement<{0}>: Could not find - {1}", typeof(TParentType), AutomationIdentifier.ToIdOrName()));
                return null;
            }

            return element;
        }
    }
}