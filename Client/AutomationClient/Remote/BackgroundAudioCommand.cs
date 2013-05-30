using System;
using System.Text;
using Microsoft.Phone.BackgroundAudio;
using Microsoft.Xna.Framework.Media;
using XnaMediaPlayer = Microsoft.Xna.Framework.Media.MediaPlayer;

namespace WindowsPhoneTestFramework.Client.AutomationClient.Remote
{
    public partial class BackgroundAudioCommand
    {
        #region Methods

        protected override void DoImpl()
        {
            bool success = false;
            StringBuilder message = new StringBuilder();

            try
            {
                ////XnaMediaPlayer.Play(Song.FromUri("Dummy", new Uri("http://dummy:0/dummy.dum")));
                XnaMediaPlayer.Stop();
                success = true;
            }
            catch (Exception e)
            {
                message.AppendLine("XnaMediaPlayer exception:");
                message.AppendLine(e.ToString());
            }

            try
            {
                ////BackgroundAudioPlayer.Instance.Track = null;
                BackgroundAudioPlayer.Instance.Close();
                BackgroundAudioPlayer.Instance.Stop();
                success = true;
            }
            catch (Exception e)
            {
                message.AppendLine("BackgroundAudioPlayer exception:");
                message.AppendLine(e.ToString());
            }

            if (success)
            {
                SendSuccessResult();
            }
            else
            {
                SendExceptionFailedResult(new Exception(message.ToString()));
            }
        }

        #endregion
    }
}
