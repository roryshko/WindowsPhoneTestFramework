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
                SendNotFoundResult(string.Format("GetApplicationSettingCommand: Could not find the key :{0} in AppSettings", Key));
            }

            var value = AppSettings[Key];
            SendTextResult(value.ToString());
        }
    }
}
