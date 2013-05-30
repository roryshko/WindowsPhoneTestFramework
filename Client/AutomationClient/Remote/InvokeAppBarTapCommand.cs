using System;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace WindowsPhoneTestFramework.Client.AutomationClient.Remote
{
    using System.Linq;
    using WindowsPhoneTestFramework.Client.AutomationClient.Helpers;
    using System.Windows.Automation.Provider;

    public partial class InvokeAppBarTapCommand : AutomationElementCommandBase
    {
        protected override void DoImpl()
        {
            var rootVisual = AutomationElementFinder.GetRootVisual() as PhoneApplicationFrame;
            if (rootVisual == null)
            {
                SendNotFoundResult("InvokeAppBarTapCommand: Could not find the RootVisual");
                return;
            }
            
            var page = rootVisual.Content as PhoneApplicationPage;
            if (page == null)
            {
                SendNotFoundResult("InvokeAppBarTapCommand: Could not find the Page");
                return;
            }

            var appBar = page.ApplicationBar;
            if (appBar == null)
            {
                Thread.Sleep(TimeSpan.FromSeconds(5));
                appBar = page.ApplicationBar;

                if (appBar == null)
                {
                    SendNotFoundResult("InvokeAppBarTapCommand: Could not find the Application bar");
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
                var invokableIcon = appIcon as IInvokeProvider;
                if (invokableIcon != null)
                {
                    invokableIcon.Invoke();
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
                var invokableMenu = appMenu as IInvokeProvider;
                if (invokableMenu != null)
                {
                    invokableMenu.Invoke();
                    SendSuccessResult();
                    return;
                }
            }

            // if reached here, then failed
            SendNotFoundResult(string.Format("InvokeAppBarTapCommand: Could not find {0} in Application bar", applicationBarText));
        }
    }
}