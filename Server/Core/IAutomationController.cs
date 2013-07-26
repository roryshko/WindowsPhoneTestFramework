//  ----------------------------------------------------------------------
//  <copyright file="IAutomationController.cs" company="Expensify">
//      (c) Copyright Expensify. http://www.expensify.com
//      This source is subject to the Microsoft Public License (Ms-PL)
//      Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//      All other rights reserved.
//  </copyright>
//  
//  Author - Stuart Lodge, Cirrious. http://www.cirrious.com
//  ------------------------------------------------------------------------

using System;
using WindowsPhoneTestFramework.Server.Utils;

namespace WindowsPhoneTestFramework.Server.Core
{
    public interface IAutomationController : IDisposable, ITrace
    {
        string Name { get; }
        string Version { get; }

        // TODO - add a new interface for Choosers/Pickers? Or for advanced tasks like that...

        void Start(string initialisationString = null,
                   AutomationIdentification automationIdentification = AutomationIdentification.TryEverything);

        void Stop();
        void ShutDown();

        IApplicationAutomationController ApplicationAutomationController { get; }
        IDeviceController DeviceController { get; }
        IDisplayInputController DisplayInputController { get; }
    }

    public enum WindowsPhoneVersion
    {
        Seven,
        Eight
    }
}