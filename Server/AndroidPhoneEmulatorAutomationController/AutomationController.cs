//  ----------------------------------------------------------------------
//  <copyright file="AutomationController.cs" company="Expensify">
//      (c) Copyright Expensify. http://www.expensify.com
//      This source is subject to the Microsoft Public License (Ms-PL)
//      Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//      All other rights reserved.
//  </copyright>
//  
//  Author - Stuart Lodge, Cirrious. http://www.cirrious.com
//  ------------------------------------------------------------------------

using System;
using WindowsPhoneTestFramework.Server.AndroidDeviceController;
using WindowsPhoneTestFramework.Server.Core;
using WindowsPhoneTestFramework.Server.Utils;
using WindowsPhoneTestFramework.Server.WCFHostedAutomationController;

namespace WindowsPhoneTestFramework.Server.AutomationController.Android.Emulator
{
    public class AutomationController : TraceBase, IAutomationController
    {
        public static string DefaultBindingAddress = "http://localhost:8085/phoneAutomation";

        public string Name
        {
            get { return "Android Emulator"; }
        }

        public string Version
        {
            get { return "0.1"; }
        }

        // not started counting yet!

        private ServiceHostController _hostController;

        public IApplicationAutomationController ApplicationAutomationController
        {
            get { return _hostController == null ? null : _hostController.AutomationController; }
        }

        public IDeviceController DeviceController { get; set; }

        public IDisplayInputController DisplayInputController
        {
            get { return DeviceController.DisplayInputController; }
        }

        public void Dispose()
        {
            Stop();
            ShutDown();
            GC.SuppressFinalize(this);
        }

        public void Start(string initialisationString = null,
                          AutomationIdentification automationIdentification = AutomationIdentification.TryEverything)
        {
            if (_hostController != null)
                throw new InvalidOperationException("hostController already initialised");

            if (DeviceController != null)
                throw new InvalidOperationException("Driver already initialised");

            var parsedInitialisationString = new ParsedInitialisationString(initialisationString);

            var bindingAddressUrl = parsedInitialisationString.SafeGetValue("BindingAddress");
            var bindingAddressUri =
                new Uri(string.IsNullOrEmpty(bindingAddressUrl) ? DefaultBindingAddress : bindingAddressUrl);

            StartDriver(parsedInitialisationString);
            StartPhoneAutomationController(automationIdentification, bindingAddressUri);
        }

        private void StartDriver(ParsedInitialisationString parsedInitialisationString)
        {
            var driverConfiguration = new AdbDeviceControllerConfiguration(parsedInitialisationString.Fields);
            var driver = new AdbDeviceController(driverConfiguration);
            driver.Trace += (sender, args) => InvokeTrace(args);
            if (!driver.TryConnect())
                throw new AutomationException("Unable to connect to Android emulator driver");
            DeviceController = driver;
        }

        protected void StartPhoneAutomationController(AutomationIdentification automationIdentification,
                                                      Uri bindingAddress)
        {
            try
            {
                var hostController = new ServiceHostController
                    {
                        AutomationIdentification = automationIdentification,
                    };

                hostController.Trace += (sender, args) => InvokeTrace(args);

                hostController.Start(bindingAddress);

                _hostController = hostController;
            }
            catch (Exception exception)
            {
                throw new AutomationException("Failed to start ApplicationAutomationController", exception);
            }
        }

        public void Start(WindowsPhoneVersion version, string initialisationString = null,
                          AutomationIdentification automationIdentification = AutomationIdentification.TryEverything)
        {
            throw new NotImplementedException();
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
                ShutDown();
                DeviceController = null;
            }
        }


        public void ShutDown()
        {
            if (DeviceController != null)
            {
                DeviceController.ForceDeviceShutDown();
            }
        }
    }
}