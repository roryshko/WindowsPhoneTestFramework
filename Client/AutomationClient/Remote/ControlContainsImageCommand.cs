using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using WindowsPhoneTestFramework.Client.AutomationClient.Helpers;

namespace WindowsPhoneTestFramework.Client.AutomationClient.Remote
{
    public partial class ControlContainsImageCommand
    {
        protected override void DoImpl()
        {
            var element = GetUIElement(true);
            if (element == null)
            {
                return;
            }
            

            var image = AutomationElementFinder.GetElementProperty<Image>(element, "Content");

            if (image!=null)
            {
               if( image.Name == ImageName)
               {
                   SendSuccessResult();
               }
               else
               {
                   SendNotFoundResult(string.Format("ControlContainsImageCommand: Could not find the image {0}", ImageName));
               }
            }

        }
    }
}
