//  ----------------------------------------------------------------------
//  <copyright file="GetApplicationSettingsCommand.cs" company="Expensify">
//      (c) Copyright Expensify. http://www.expensify.com
//      This source is subject to the Microsoft Public License (Ms-PL)
//      Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//      All other rights reserved.
//  </copyright>
//  
//  Author - Stuart Lodge, Cirrious. http://www.cirrious.com
//  ------------------------------------------------------------------------

using System.IO.IsolatedStorage;
using System.Linq;

namespace WindowsPhoneTestFramework.Client.AutomationClient.Remote
{
    public partial class GetApplicationSettingsCommand
    {
        private static readonly IsolatedStorageSettings AppSettings = IsolatedStorageSettings.ApplicationSettings;

        protected override void DoImpl()
        {
            // This is bad. Should Serialise the value first (first draft).
            SendDictionaryResult(
                AppSettings
                    .Where(pair => pair.Value is string)
                    .ToDictionary(pair => pair.Key, pair => pair.Value.ToString()));
        }
    }
}