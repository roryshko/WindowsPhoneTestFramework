//  ----------------------------------------------------------------------
//  <copyright file="SetApplicationSettingCommand.cs" company="Expensify">
//      (c) Copyright Expensify. http://www.expensify.com
//      This source is subject to the Microsoft Public License (Ms-PL)
//      Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//      All other rights reserved.
//  </copyright>
//  
//  Author - Stuart Lodge, Cirrious. http://www.cirrious.com
//  ------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO.IsolatedStorage;

namespace WindowsPhoneTestFramework.Client.AutomationClient.Remote
{
    public partial class SetApplicationSettingCommand
    {
        private static readonly IsolatedStorageSettings AppSettings = IsolatedStorageSettings.ApplicationSettings;

        protected override void DoImpl()
        {
            AppSettings[Key] = Value;
            AppSettings.Save();
            var settings = new Dictionary<string, string>();
            settings.Add(Key, Value);
            AutomationClient.RaiseApplicationSettingsChanged(settings);
            SendSuccessResult();
        }
    }
}