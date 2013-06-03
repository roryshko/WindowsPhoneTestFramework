// ----------------------------------------------------------------------
// <copyright file="SetValueCommand.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using System;
using WindowsPhoneTestFramework.Client.AutomationClient.Helpers;

namespace WindowsPhoneTestFramework.Client.AutomationClient.Remote
{
    public partial class SetValueCommand
    {
        protected override void DoImpl()
        {
            var element = GetUIElement();
            if (element == null)
            {
                return;
            }

            if (ValueCommandHelper.TrySetValue(element, TextValue))
            {
                SendSuccessResult();
                return;
            }

            if (AutomationElementFinder.SetElementProperty<string>(element, "Text", TextValue))
            {
                SendSuccessResult();
                return;
            }

            if (AutomationElementFinder.SetElementProperty<string>(element, "Password", TextValue))
            {
                SendSuccessResult();
                return;
            }

            bool boolValue;
            if (bool.TryParse(TextValue, out boolValue))
            {
                if (AutomationElementFinder.SetElementProperty<bool>(element, "IsChecked", boolValue))
                {
                    SendSuccessResult();
                    return;
                }
            }

            int intValue;
            if (int.TryParse(TextValue, out intValue))
            {
                if (AutomationElementFinder.SetElementProperty<int>(element, "Value", intValue))
                {
                    SendSuccessResult();
                    return;
                }
            }

            double doubleValue;
            if (double.TryParse(TextValue, out doubleValue))
            {
                if (AutomationElementFinder.SetElementProperty<double>(element, "Value", doubleValue))
                {
                    SendSuccessResult();
                    return;
                }
            }

            DateTime dateTimeValue;
            if (DateTime.TryParse(TextValue, out dateTimeValue))
            {
                if (AutomationElementFinder.SetElementProperty<DateTime>(element, "Value", dateTimeValue))
                {
                    SendSuccessResult();
                    return;
                }

                if (AutomationElementFinder.SetElementProperty<DateTime?>(element, "Value", dateTimeValue))
                {
                    SendSuccessResult();
                    return;
                }
            }

            if (AutomationElementFinder.SetElementProperty<string>(element, "Value", TextValue))
            {
                SendSuccessResult();
                return;
            }



            // if text, password IsChecked, Value are all missing... then give up
            SendNotFoundResult(string.Format("SetValueCommand: Could not set the value :{0} in control :{1}", TextValue, AutomationIdentifier.ToIdOrName()));
        }
    }
}