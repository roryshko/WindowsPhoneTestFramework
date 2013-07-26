//  ----------------------------------------------------------------------
//  <copyright file="IValueManipulator.cs" company="Expensify">
//      (c) Copyright Expensify. http://www.expensify.com
//      This source is subject to the Microsoft Public License (Ms-PL)
//      Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//      All other rights reserved.
//  </copyright>
//  
//  Author - Stuart Lodge, Cirrious. http://www.cirrious.com
//  ------------------------------------------------------------------------

using System.Windows;

namespace WindowsPhoneTestFramework.Client.AutomationClient.Helpers
{
    public interface IValueManipulator
    {
        bool TryGetValue(UIElement element, out string value);
        bool TrySetValue(UIElement element, string value);
    }
}