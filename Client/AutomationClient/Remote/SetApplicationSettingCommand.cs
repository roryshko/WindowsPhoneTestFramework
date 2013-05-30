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
