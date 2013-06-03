using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

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
