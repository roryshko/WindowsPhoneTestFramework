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
using System.Configuration;
using System.Threading;

using WindowsPhoneTestFramework.Server.Core;
using WindowsPhoneTestFramework.Server.Utils;
using WindowsPhoneTestFramework.Server.WCFHostedAutomationController;
using WindowsPhoneTestFramework.Server.WindowsPhoneDeviceController;

namespace WindowsPhoneTestFramework.Server.AutomationController.WindowsPhone.Emulator
{
    public class AutomationController : TraceBase, IAutomationController
    {
        private const string DefaultPort = "8085";

        private string _name;
        public string Name
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_name))
                {
                    GetName();
                }

                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public string Port
        {
            get
            {
                if (string.IsNullOrEmpty(_port))
                {
                    GetName();
                }

                return _port;
            }
        }

        private void GetName()
        {
            var names = ConfigurationManager.AppSettings.Get("EmuSteps.Application.WindowsPhone.TargetDevice");
            if (string.IsNullOrWhiteSpace(names))
            {
                _name = "Emulator";
                _port = DefaultPort;
            }

            else
            {
                var pairs = names.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                GetNextAvailableDevice(pairs, 0);

                if (string.IsNullOrEmpty(_name))
                {
                    GetNextAvailableDevice(pairs, 300000);
                }

                if (string.IsNullOrEmpty(_name))
                {
                    throw new InvalidOperationException("Timed out waiting for an emulator to be available... Too many tests running in parallel.");
                }
            }
        }

        private void GetNextAvailableDevice(string[] pairs, int timeout)
        {
            foreach (var pair in pairs)
            {
                var mut = new Mutex(false, pair);
                try
                {
                    if (mut.WaitOne(timeout))
                    {
                        PopulateNames(pair, mut);

                        break;
                    }
                }
                catch (AbandonedMutexException)
                {
                    // If we get an abandoned mutex exception, it's unlikely to impact on us, we don't persist much

                    mut.Dispose();
                }
            }
        }

        private void PopulateNames(string pair, Mutex mut)
        {
            var split = pair.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (split.Length == 1)
            {
                _name = split[0];
                _port = DefaultPort;

                return;
            }
            else if (split.Length != 2)
            {
                throw new InvalidOperationException("Config set up incorrectly for target device");
            }

            _name = split[0];
            _port = split[1];

            _mutex = mut;
        }

        public string Version { get { return "0.1"; } } // not started counting yet!

        private ServiceHostController _hostController;

        private string _port;

        private Mutex _mutex;

        public IApplicationAutomationController ApplicationAutomationController { get { return _hostController == null ? null : _hostController.AutomationController; } }
        public IDeviceController DeviceController { get; set; }
        public IDisplayInputController DisplayInputController { get { return DeviceController.DisplayInputController; } }

        public void Dispose()
        {
            if (_mutex != null)
            {
                _mutex.ReleaseMutex();
                _mutex.Dispose();
            }

            Stop();
            GC.SuppressFinalize(this);
        }

        public void Start(string initialisationString = null, AutomationIdentification automationIdentification = AutomationIdentification.TryEverything)
        {
            if (_hostController != null)
                throw new InvalidOperationException("hostController already initialised");

            if (DeviceController != null)
                throw new InvalidOperationException("Driver already initialised");

            var bindingAddressUri = string.IsNullOrEmpty(initialisationString) ? BuildBindingAddress() : new Uri(initialisationString);

            StartDriver();
            StartPhoneAutomationController(automationIdentification, bindingAddressUri);
        }

        public static string DefaultBindingAddress = "http://localhost:{0}/phoneAutomation";

        private Uri BuildBindingAddress()
        {
            return new Uri(string.Format(DefaultBindingAddress, this.Port));
        }

        public static WindowsPhoneVersion ExecutingEmulatorVersion
        {
            get { return typeof(Microsoft.SmartDevice.Connectivity.Platform).Assembly.GetName().Version.Major == 11 ? WindowsPhoneVersion.Eight : WindowsPhoneVersion.Seven; }
        }

        private void StartDriver()
        {
            IDeviceController driver = null;
            if (Name.Equals("Device", StringComparison.InvariantCulture))
            {
                driver = new PhoneWindowsPhoneDeviceController();
            }
            else
            {
                driver = ExecutingEmulatorVersion == WindowsPhoneVersion.Seven
                                 ? new EmulatorWindowsPhoneDeviceController(Name)
                                 : new Win8EmulatorWindowsPhoneDeviceController(Name, Port);
            }

            driver.Trace += (sender, args) => InvokeTrace(args);
            if (!driver.TryConnect())
            {
                throw new AutomationException("Unable to connect to driver");
            }

            DeviceController = driver;
        }

        protected void StartPhoneAutomationController(AutomationIdentification automationIdentification, Uri bindingAddress)
        {
            try
            {
                var hostController = new ServiceHostController()
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
