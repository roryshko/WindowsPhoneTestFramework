//  ----------------------------------------------------------------------
//  <copyright file="GetColorCommand.cs" company="Expensify">
//      (c) Copyright Expensify. http://www.expensify.com
//      This source is subject to the Microsoft Public License (Ms-PL)
//      Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//      All other rights reserved.
//  </copyright>
//  
//  Author - Stuart Lodge, Cirrious. http://www.cirrious.com
//  ------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WindowsPhoneTestFramework.Client.AutomationClient.Helpers;

namespace WindowsPhoneTestFramework.Client.AutomationClient.Remote
{
    public partial class GetColorCommand
    {
        protected override void DoImpl()
        {
            var element = GetFrameworkElement();

            if (element == null)
            {
                element =
                    (FrameworkElement)
                    AutomationElementFinder.FindElementByDisplayedText(AutomationIdentifier.AutomationName);
            }

            if (element != null)
            {
                //Find out its background color
                if (element is Border)
                {
                    var elementControl = (Border) element;

                    if (elementControl.Background is SolidColorBrush)
                    {
                        SendColorResult(((SolidColorBrush) elementControl.Background).Color.ToString());
                        return;
                    }
                }
            }

            SendNotFoundResult(string.Format("GetColorCommand: Could not find the element - {0}",
                                             AutomationIdentifier.ToIdOrName()));
        }
    }
}