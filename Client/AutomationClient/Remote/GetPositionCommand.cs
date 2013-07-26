// ----------------------------------------------------------------------
// <copyright file="GetPositionCommand.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using System;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WindowsPhoneTestFramework.Client.AutomationClient.Helpers;

namespace WindowsPhoneTestFramework.Client.AutomationClient.Remote
{
    public partial class GetPositionCommand
    {
        protected override void DoImpl()
        {
            var element = GetFrameworkElement(false);

            var rootVisual = AutomationElementFinder.GetRootVisual() as PhoneApplicationFrame;
            if (rootVisual == null)
            {
                SendNotFoundResult("GetPositionCommand: RootVisual is null");
                return;
            }

            var page = rootVisual.Content as PhoneApplicationPage;
            if (page == null)
            {
                SendNotFoundResult("GetPositionCommand: Page is null");
                return;
            }

            if (element == null)
            {
                if (!AutomationIdentifier.DisplayedText.Contains("applicationBar|"))
                {
                    SendNotFoundResult(string.Format("GetPositionCommand: Displayed text : {0} not found", AutomationIdentifier.DisplayedText));
                    return;
                }

                var appBar = page.ApplicationBar;
                if (appBar == null)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(2));

                    appBar = page.ApplicationBar;

                    if (appBar == null)
                    {
                        SendNotFoundResult("GetPositionCommand: AppBar null");
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

#warning This method is far too long/flat/big - needs refactoring
                var applicationBarText = AutomationIdentifier.DisplayedText.Replace("applicationBar|", "");
                var menuItemIndex = 0;
                var iconButtonIndex = 0;
                var menuItemMatch = -1;
                var iconButtonMatch = -1;
                var left = 0;
                var right = 450;
                var top = 800;
                var bottom = 800;
                if ((buttons != null && buttons.Count != 0))
                    foreach (var button in buttons)
                    {
                        var appIcon = button as ApplicationBarIconButton;
                        if (appIcon != null)
                        {
                            if (appIcon.Text == applicationBarText)
                            {
                                iconButtonMatch = iconButtonIndex;
                            }

                            iconButtonIndex++;
                        }
                    }

                if ((menuItems != null && menuItems.Count != 0))
                    foreach (var menuItem in menuItems)
                    {
                        var appMenu = menuItem as ApplicationBarMenuItem;
                        if (appMenu != null)
                        {
                            if (appMenu.Text == applicationBarText)
                            {
                                menuItemMatch = menuItemIndex;
                            }

                            menuItemIndex++;
                        }
                    }

                if (iconButtonMatch >= 0 || menuItemMatch >= 0)
                {
                    if (iconButtonMatch > -1)
                    {
                        // It's icons so move across to left or right
                        left = 225 - (iconButtonIndex * 25) + (iconButtonMatch * 50);
                        right = left + 50;
                    }

                    top -= (int)(((menuItemIndex - menuItemMatch) * appBar.DefaultSize) + appBar.MiniSize);
                    bottom -= (int)(((menuItemIndex - menuItemMatch + 1) * appBar.DefaultSize) + appBar.MiniSize);

                    SendPositionResult(left, top, right - left, bottom - top);
                }
                else
                {
                    SendNotFoundResult(string.Format("GetPositionCommand: Could not find {0} in AppBar", applicationBarText));
                }

                return;
            }

            // if element is not visible, then return an empty position
            if (ReturnEmptyIfNotVisible)
                if (element.Visibility == Visibility.Collapsed)
                {
                    SendPositionResult(0.0, 0.0, 0.0, 0.0);
                    return;
                }

            try
            {
                // this answer is based on answer in http://forums.silverlight.net/t/12160.aspx
                // please note that for some weird transformations (skewing while rotating while... ) then it 
                // may not yield perfect answers...

                var position = AutomationElementFinder.Position(element);
                if (position.IsEmpty)
                {
                    SendNotFoundResult(string.Format("GetPositionCommand: Empty result for {0}, Page {1}", AutomationIdentifier.ElementName, page.Title));
                }
                else
                {
                    SendPositionResult(position.Left, position.Top, position.Width, position.Height);
                }
            }
            catch (Exception exc)
            {
                // TODO - could log the exception
                SendNotFoundResult(string.Format("GetPositionCommand: exception {1}, {2} when looking for {0}.  Page {3}", AutomationIdentifier.ElementName, exc.GetType().Name, exc.Message, page.Title));
            }
        }
    }
}