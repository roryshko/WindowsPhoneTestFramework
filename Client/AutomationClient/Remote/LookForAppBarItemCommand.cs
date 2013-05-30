using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WindowsPhoneTestFramework.Client.AutomationClient.Helpers;

namespace WindowsPhoneTestFramework.Client.AutomationClient.Remote
{
    public partial class LookForAppBarItemCommand : AutomationElementCommandBase
    {
        protected override void DoImpl()
        {
            var rootVisual = AutomationElementFinder.GetRootVisual() as PhoneApplicationFrame;
            if (rootVisual == null)
            {
                SendNotFoundResult("LookForAppBarItemCommand: Could not find the RootVisual");
                return;
            }

            var page = rootVisual.Content as PhoneApplicationPage;
            if (page == null)
            {
                SendNotFoundResult("LookForAppBarItemCommand: Could not find the Page");
                return;
            }

            var appBar = page.ApplicationBar;
            if (appBar == null)
            {
                Thread.Sleep(TimeSpan.FromSeconds(5));
                appBar = page.ApplicationBar;

                if (appBar == null)
                {
                    SendNotFoundResult("LookForAppBarItemCommand: Could not find the Application bar");
                    return;
                }
            }

            if (!appBar.IsVisible)
            {
                SendPositionResult(0.0, 0.0, 0.0, 0.0);
                return;
            }

            var buttons = appBar.Buttons;
            var menuItems = appBar.MenuItems;

            var applicationBarText = AutomationIdentifier.DisplayedText;

            var buttonsExist = buttons != null && buttons.Count != 0;

            if (!buttonsExist)
            {
                Thread.Sleep(TimeSpan.FromSeconds(5));
                buttonsExist = buttons != null && buttons.Count != 0;
            }

            if (buttonsExist)
            {
                var appIcon =
                    buttons.OfType<ApplicationBarIconButton>().FirstOrDefault(icon => icon.Text.ToLowerInvariant() == applicationBarText.ToLowerInvariant());

                if (appIcon != null)
                {
                    SendSuccessResult();
                    return;
                }
            }

            var menuItemsExist = menuItems != null && menuItems.Count != 0;

            if (!menuItemsExist)
            {
                Thread.Sleep(TimeSpan.FromSeconds(5));
                menuItemsExist = menuItems != null && menuItems.Count != 0;
            }

            if (menuItemsExist)
            {
                var appMenu =
                    menuItems.OfType<ApplicationBarMenuItem>().FirstOrDefault(menu => menu.Text.ToLowerInvariant() == applicationBarText.ToLowerInvariant());

                if (appMenu != null)
                {
                    SendSuccessResult();
                    return;
                }
            }

            // if reached here, then failed
            SendNotFoundResult(string.Format("LookForAppBarItemCommand: Could not find {0} in Application bar", applicationBarText));
        }
    }
}
