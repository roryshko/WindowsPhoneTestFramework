// ----------------------------------------------------------------------
// <copyright file="AutomationController.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using System;
using WindowsPhoneTestFramework.Server.Core;
using WindowsPhoneTestFramework.Server.Utils;
using WindowsPhoneTestFramework.Server.WCFHostedAutomationController;
using WindowsPhoneTestFramework.Server.WindowsPhoneDeviceController;

namespace WindowsPhoneTestFramework.Server.AutomationController.WindowsPhone.Emulator
{
    public class AutomationController : TraceBase, IAutomationController
    {
        private ServiceHostController _hostController;

        public IApplicationAutomationController ApplicationAutomationController { get { return _hostController == null ? null : _hostController.AutomationController; } }
        public IDeviceController DeviceController { get; set; }
        public IDisplayInputController DisplayInputController { get { return DeviceController.DisplayInputController; } }

        public void Dispose()
        {
            Stop();
            GC.SuppressFinalize(this);
        }

        public void Start(string initialisationString = null, AutomationIdentification automationIdentification = AutomationIdentification.TryEverything)
        {
            if (_hostController != null)
                throw new InvalidOperationException("hostController already initialised");

            if (DeviceController != null)
                throw new InvalidOperationException("Driver already initialised");

            var bindingAddressUri = string.IsNullOrEmpty(initialisationString) ? null : new Uri(initialisationString);

            StartDriver();
            StartPhoneAutomationController(automationIdentification, bindingAddressUri);
        }

        private void StartDriver()
        {
            var driver = new EmulatorWindowsPhoneDeviceController();
            driver.Trace += (sender, args) => InvokeTrace(args);
            if (!driver.TryConnect())
                throw new AutomationException("Unable to connect to emulator driver");
            DeviceController = driver;
        }

        private void StartPhoneAutomationController(AutomationIdentification automationIdentification, Uri bindingAddress)
        {
            try
            {
                var hostController = new ServiceHostController()
                {
                    AutomationIdentification = automationIdentification,
                };

                if (bindingAddress != null)
                    hostController.BindingAddress = bindingAddress;

                hostController.Trace += (sender, args) => InvokeTrace(args);

                hostController.Start();

                _hostController = hostController;
            }
            catch (Exception exception)
            {
                throw new AutomationException("Failed to start ApplicationAutomationController", exception);
            }
        }

        public void Stop()
        {
            if (_hostController != null)
            {
                _hostController.Stop();
                _hostController = null;
            }
            if (DeviceController != null)
            {
                DeviceController.ReleaseDeviceConnection();
                DeviceController = null;
            }
        }
    }
}
