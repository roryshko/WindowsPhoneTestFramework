using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Microsoft.Phone.Controls;
using WindowsPhoneTestFramework.Client.AutomationClient.Helpers;

namespace WindowsPhoneTestFramework.Client.AutomationClient.Remote
{
    partial class LookForTypeCommand
    {
        protected override void DoImpl()
        {
            var e = AutomationElementFinder.GetRootVisual();

            switch (TypeStr)
            {
                case "Microsoft.Phone.Controls.Panorama":
                    if (FindVisualChildren<Panorama>(e).Any())
                    {
                        SendSuccessResult();
                        return;
                    }
                    break;
                case "System.Windows.Controls.Primitives.Popup":
                    if (FindVisualChildren<Popup>(e).Any())
                    {
                        SendSuccessResult();
                        return;
                    }
                    break;
            }

            SendNotFoundResult( string.Format("{0} not found", TypeStr ));
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T) child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
    }
}