//  ----------------------------------------------------------------------
//  <copyright file="GetIsCheckedCommand.cs" company="Expensify">
//      (c) Copyright Expensify. http://www.expensify.com
//      This source is subject to the Microsoft Public License (Ms-PL)
//      Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//      All other rights reserved.
//  </copyright>
//  
//  Author - Stuart Lodge, Cirrious. http://www.cirrious.com
//  ------------------------------------------------------------------------

using System;
using System.Linq;
using System.Windows;

namespace WindowsPhoneTestFramework.Client.AutomationClient.Remote
{
    /// <summary>
    /// The get visual state command.
    /// </summary>
    public partial class GetCheckedStatusCommand
    {
        #region Methods

        /// <summary>
        /// The do impl.
        /// </summary>
        protected override void DoImpl()
        {
            FrameworkElement element = GetFrameworkElement();

            var checkedProperty = element.GetType().GetProperties().FirstOrDefault(p => p.Name == "IsChecked");

            if (checkedProperty != null)
            {
                var isChecked = checkedProperty.GetValue(element, null);

                try
                {
                    var val = (bool) isChecked;
                    SendTextResult(val.ToString());
                    return;
                }
                catch (InvalidCastException)
                {
                    SendNotFoundResult("Couldn't find the correct property.");
                    return;
                }
            }

            SendNotFoundResult("Couldn't find element.");
        }

        #endregion
    }
}