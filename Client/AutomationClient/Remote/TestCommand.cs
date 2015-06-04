using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Automation.Peers;
using Microsoft.Phone.Controls;
using WindowsPhoneTestFramework.Client.AutomationClient.Helpers;

namespace WindowsPhoneTestFramework.Client.AutomationClient.Remote
{
    public partial class TestCommand
    {
        protected override void DoImpl()
        {
            UIElement element;

            if (AutomationIdentifier == null)
            {
                var e = AutomationElementFinder.GetRootVisual() ;
                element = ((PhoneApplicationFrame)e).Content as PhoneApplicationPage;
            }
            else
            {
                element = GetUIElement();

                if (element == null)
                {
                    SendNotFoundResult("Couldn't find element " + AutomationIdentifier.ElementName);
                    return;
                }
            }

            var fe = FrameworkElementAutomationPeer.CreatePeerForElement(element);

            var peer = fe as TestCommandPeer;

            if (peer != null)
            {
                var result = peer.Command(Command, Data);
                SendTextResult(result);
            }
            else
                throw new TestAutomationException("No automation peer found for " + element.GetType()
                                                                                           .FullName);
        }
    }
}