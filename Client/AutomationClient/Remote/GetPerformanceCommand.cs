//  ----------------------------------------------------------------------
//  <copyright file="GetPerformanceCommand.cs" company="Expensify">
//      (c) Copyright Expensify. http://www.expensify.com
//      This source is subject to the Microsoft Public License (Ms-PL)
//      Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//      All other rights reserved.
//  </copyright>
//  
//  Author - Stuart Lodge, Cirrious. http://www.cirrious.com
//  ------------------------------------------------------------------------

using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using Microsoft.Phone.Info;

namespace WindowsPhoneTestFramework.Client.AutomationClient.Remote
{
    public partial class GetPerformanceCommand
    {
        protected override void DoImpl()
        {
            var values = new Dictionary<string, string>();

            values["deviceResolution"] = string.Format("{0}x{1}", Application.Current.Host.Content.ActualHeight,
                                                       Application.Current.Host.Content.ActualWidth);
            values["currentMemUsage"] = DeviceStatus.ApplicationCurrentMemoryUsage.ToString(CultureInfo.InvariantCulture);
            values["peakMemUsage"] = DeviceStatus.ApplicationPeakMemoryUsage.ToString(CultureInfo.InvariantCulture);
            values["memUsageLimit"] = DeviceStatus.ApplicationMemoryUsageLimit.ToString(CultureInfo.InvariantCulture);
            values["deviceTotalMem"] = DeviceStatus.DeviceTotalMemory.ToString(CultureInfo.InvariantCulture);

            SendDictionaryResult(values);
        }
    }
}