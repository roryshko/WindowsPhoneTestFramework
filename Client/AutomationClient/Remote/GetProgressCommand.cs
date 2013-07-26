// ----------------------------------------------------------------------
// <copyright file="GetProgressCommand.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using System;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace WindowsPhoneTestFramework.Client.AutomationClient.Remote
{
    public partial class GetProgressCommand
    {
        protected override void DoImpl()
        {
            var element = GetFrameworkElement(false);

            // if element is not visible, then return an empty position
            if (ReturnEmptyIfNotVisible)
            {
                if (element.Visibility == Visibility.Collapsed)
                {
                    SendProgressResult(double.MinValue, double.MinValue, double.MinValue);
                    return;
                }
            }

            var progress = element as System.Windows.Controls.Primitives.RangeBase;
            try
            {
                SendProgressResult(
                    progress.Minimum,
                    progress.Maximum,
                    progress.Value                    
                );
            }
            catch (Exception exc)
            {
                // TODO - could log the exception
                SendExceptionFailedResult(exc);
            }
        }
    }
}