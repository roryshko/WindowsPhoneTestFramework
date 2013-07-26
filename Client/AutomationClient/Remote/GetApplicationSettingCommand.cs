//  ----------------------------------------------------------------------
//  <copyright file="GetApplicationSettingCommand.cs" company="Expensify">
//      (c) Copyright Expensify. http://www.expensify.com
//      This source is subject to the Microsoft Public License (Ms-PL)
//      Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//      All other rights reserved.
//  </copyright>
//  
//  Author - Stuart Lodge, Cirrious. http://www.cirrious.com
//  ------------------------------------------------------------------------

using System.IO.IsolatedStorage;

namespace WindowsPhoneTestFramework.Client.AutomationClient.Remote
{
    public partial class GetApplicationSettingCommand
    {
        private static readonly IsolatedStorageSettings AppSettings = IsolatedStorageSettings.ApplicationSettings;

        protected override void DoImpl()
        {
            if (!AppSettings.Contains(Key))
            {
                SendNotFoundResult(
                    string.Format("GetApplicationSettingCommand: Could not find the key :{0} in AppSettings", Key));
            }

            var value = AppSettings[Key];
            SendTextResult(value.ToString());
        }
    }
}