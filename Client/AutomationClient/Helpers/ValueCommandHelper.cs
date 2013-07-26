//  ----------------------------------------------------------------------
//  <copyright file="ValueCommandHelper.cs" company="Expensify">
//      (c) Copyright Expensify. http://www.expensify.com
//      This source is subject to the Microsoft Public License (Ms-PL)
//      Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//      All other rights reserved.
//  </copyright>
//  
//  Author - Stuart Lodge, Cirrious. http://www.cirrious.com
//  ------------------------------------------------------------------------

using System.Collections.Generic;
using System.Windows;

namespace WindowsPhoneTestFramework.Client.AutomationClient.Helpers
{
    public static class ValueCommandHelper
    {
        public static readonly List<IValueManipulator> ValueManipulators = new List<IValueManipulator>();

        public static void AddManipulator(IValueManipulator valueManipulator)
        {
            lock (ValueManipulators)
            {
                ValueManipulators.Add(valueManipulator);
            }
        }

        public static bool TryGetValue(UIElement element, out string value)
        {
            lock (ValueManipulators)
            {
                foreach (var manipulator in ValueManipulators)
                {
                    if (manipulator.TryGetValue(element, out value))
                        return true;
                }
            }

            value = null;
            return false;
        }

        public static bool TrySetValue(UIElement element, string value)
        {
            lock (ValueManipulators)
            {
                foreach (var manipulator in ValueManipulators)
                {
                    if (manipulator.TrySetValue(element, value))
                        return true;
                }
            }

            return false;
        }
    }
}