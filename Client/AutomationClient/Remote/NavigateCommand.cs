﻿using System;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace WindowsPhoneTestFramework.Client.AutomationClient.Remote
{
    using System.Linq;
    using WindowsPhoneTestFramework.Client.AutomationClient.Helpers;
    using Microsoft.Phone.Tasks;

    public partial class NavigateCommand
    {
        protected override void DoImpl()
        {
            var rootVisual = AutomationElementFinder.GetRootVisual() as PhoneApplicationFrame;
            if (rootVisual == null)
            {
                SendNotFoundResult("Navigate: Could not find the RootVisual");
                return;
            }

            var page = rootVisual.Content as PhoneApplicationPage;
            if (page == null)
            {
                SendNotFoundResult("Navigate: Could not find the Page");
                return;
            }

#warning Should Direction be a string or enumeration here?
            switch (Direction.ToLowerInvariant())
            {
                case "назад":
                case "back":
                    {
                        page.NavigationService.GoBack();
                        break;
                    }
                case "вперед":
                case "forward":
                    {
                        page.NavigationService.GoForward();
                        break;
                    }
                case "домой":
                case "home":
                    {
                        var navigationService = page.NavigationService;
                        navigationService.Navigated += NavigateBackIfYouCanOtherwiseUnsubscribe;
                        NavigateBackIfYouCanOtherwiseUnsubscribe(navigationService, new NavigationEventArgs(null, null));
                        break;
                    }
#warning Not clear what this is
                case "to start":
                    {
                        var webBrowserTask = new WebBrowserTask { Uri = new Uri("http://www.google.co.uk") };

                        // Catch user doubleclick (throws InvalidOperationException as the app is no longer active)
                        webBrowserTask.Show();
                        break;
                    }
                default:
                    {
                        SendNotFoundResult("Navigate: could not understand : " + Direction);
                        break;
                    }
            }
            
            SendSuccessResult();
        }

        void NavigateBackIfYouCanOtherwiseUnsubscribe(object sender, NavigationEventArgs e)
        {
            var navigationService = sender as NavigationService;
            if (navigationService.CanGoBack)
            {
                navigationService.GoBack();
            }
            else
            {
                navigationService.Navigated -= NavigateBackIfYouCanOtherwiseUnsubscribe;
            }
        }
    }
}