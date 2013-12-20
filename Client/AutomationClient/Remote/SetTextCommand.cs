//  ----------------------------------------------------------------------
//  <copyright file="SetTextCommand.cs" company="Expensify">
//      (c) Copyright Expensify. http://www.expensify.com
//      This source is subject to the Microsoft Public License (Ms-PL)
//      Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//      All other rights reserved.
//  </copyright>
//  
//  Author - Stuart Lodge, Cirrious. http://www.cirrious.com
//  ------------------------------------------------------------------------

using System.Windows;
using WindowsPhoneTestFramework.Client.AutomationClient.Helpers;

namespace WindowsPhoneTestFramework.Client.AutomationClient.Remote
{
    public partial class SetTextCommand
    {
        protected override void DoImpl()
        {
            UIElement element; 

            int i;
            if ( int.TryParse(AutomationIdentifier.ElementName, out i) )
            {
                // number, so this is not element name, but element position
                element = GetUIElementByPosition( i );
            }
            else
            {
                element = GetUIElement();
            }

            if (element == null)
            {
                return;
            }

            if (AutomationElementFinder.SetElementProperty(element, "TextValue", Text))
            {
                SendSuccessResult();
                return;
            }

            if (AutomationElementFinder.SetElementProperty(element, "Text", Text))
            {
                SendSuccessResult();
                return;
            }

            if (AutomationElementFinder.SetElementProperty(element, "Password", Text))
            {
                SendSuccessResult();
                return;
            }

            if (AutomationElementFinder.SetElementProperty<object>(element, "Content", Text))
            {
                SendSuccessResult();
                return;
            }

            // if text, password and content are all missing... then give up
            SendNotFoundResult(string.Format("SetTextCommand: Could not set text :{0} for control :{1}", Text,
                                             AutomationIdentifier.ToIdOrName()));
        }

    }
}