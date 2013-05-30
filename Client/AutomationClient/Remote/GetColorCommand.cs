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

namespace WindowsPhoneTestFramework.Client.AutomationClient.Remote
{
    using WindowsPhoneTestFramework.Client.AutomationClient.Helpers;

    public partial class GetColorCommand
    {
        protected override void DoImpl()
        {
            var element = GetFrameworkElement();

            if (element == null)
            {
                element = (FrameworkElement)AutomationElementFinder.FindElementByDisplayedText(AutomationIdentifier.AutomationName);
            }

            if(element != null)
            {
                //Find out its background color
                if(element is Border)
                {
                    Border elementControl = (Border)element;

                    if (elementControl.Background is SolidColorBrush)
                    {
                        SendColorResult(((SolidColorBrush)elementControl.Background).Color.ToString());
                        return;
                    }
                }
            }

            SendNotFoundResult(string.Format("GetColorCommand: Could not find the element - {0}", AutomationIdentifier.ToIdOrName()));
        }
    }
}
