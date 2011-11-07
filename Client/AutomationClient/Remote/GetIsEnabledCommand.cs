// ----------------------------------------------------------------------
// <copyright file="GetIsEnabledCommand.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using System.Windows.Controls;

namespace WindowsPhoneTestFramework.Client.AutomationClient.Remote
{
    public partial class GetIsEnabledCommand
    {
        protected override void DoImpl()
        {
            var element = GetFrameworkElement();
            if (element == null)
            {
                SendNotFoundResult();
                return;
            }

            var control = element as Control;
            if (control == null)
            {
                SendNotFoundResult();
                return;
            }

            SendTextResult(control.IsEnabled.ToString());
        }
    }
}