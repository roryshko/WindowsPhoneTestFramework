// ----------------------------------------------------------------------
// <copyright file="WindowsPhoneDeviceControllerBase.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
//     
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using Microsoft.SmartDevice.Connectivity;
using WindowsPhoneTestFramework.Server.Core;
using WindowsPhoneTestFramework.Server.Core.Results;
using WindowsPhoneTestFramework.Server.DeviceController;
using WindowsPhoneTestFramework.Server.Utils;

namespace WindowsPhoneTestFramework.Server.WindowsPhoneDeviceController
{
    // code inspired by http://justinangel.net/WindowsPhone7EmulatorAutomation - thanks Justin

    public class WindowsPhoneDeviceControllerBase : DeviceControllerBase
    {
        public string WpSdkName
        {
            get;
            set;
        }

        public string WpDeviceNameBase { get; private set; }

        public WindowsPhoneDeviceControllerBase(string deviceNameBase)
        {
            WpSdkName = "Windows Phone 7";
            WpDeviceNameBase = deviceNameBase;
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                // nothing special to do...
            }

            // release the device regardless of whether or not we are disposing
            ExceptionSafe.ExecuteConsoleWriteAnyException(
                    ReleaseDeviceConnection,
                    "exception shutting down COM driver");

            base.Dispose(isDisposing);
        }

        private Device _device;

        private Device Device
        {
            get
            {
                // return an existing device
                if (_device != null)
                    return _device;

                // Get CoreCon DataStore
                InvokeTrace("creating datastore");
                var dsmgrObj = new DatastoreManager(1033);

                // Get the plaform
                InvokeTrace("getting platform");
                var platforms = dsmgrObj.GetPlatforms();
                InvokeTrace("{0} platform(s) found", platforms.Count);
                Platform phoneSdk = platforms.Single(p => p.Name == WpSdkName);
                InvokeTrace("platform '{0}' found", WpSdkName);

                // find the device
                InvokeTrace(string.Format("looking for device '{0}'", WpDeviceNameBase));
                var devices = phoneSdk.GetDevices();
                InvokeTrace(string.Format("{0} devices found", (object) devices.Count));
                var device = phoneSdk.GetDevices().FirstOrDefault(d => d.Name == WpDeviceNameBase);

                if (device == null)
                {
                    InvokeTrace("device {0} not found - looking for similar matches", WpDeviceNameBase);
                    device = phoneSdk.GetDevices().FirstOrDefault(d => d.Name.StartsWith(WpDeviceNameBase));
                }

                if (device == null)
                {
                    InvokeTrace("device {0} not found - and no similar matches found", WpDeviceNameBase);
                    InvokeTrace("available devices were", WpDeviceNameBase);
                    foreach (var d in phoneSdk.GetDevices())
                        InvokeTrace("    " + d.Name);

                    // TODO - need a better exception than this!
                    throw new ApplicationException("Aborting - could not find device " + device);
                }

                // make the connection
                InvokeTrace("connecting to device...");
                device.Connect();
                InvokeTrace("device Connected...");

                _device = device;
                return _device;
            }
        }

        public override bool TryConnect()
        {
            try
            {
                // to ensure a connection exists, just try to connect to the device
                var device = Device;
                if (Device == null)
                    throw new InvalidOperationException("device not connected!");

                return true;
            }
            catch (Exception exception)
            {
                InvokeTrace("problem during connection {0} - {1}", exception.GetType().FullName, exception.Message);
            }

            return false;
        }

        public override void ReleaseDeviceConnection()
        {
            if (_device != null)
            {
                _device.Disconnect();
                _device = null;
            }
        }

        private WindowsPhoneApplicationDefinition ToWindowsPhoneApplicationDefinition(ApplicationDefinition applicationDefinition)
        {
            return new WindowsPhoneApplicationDefinition(applicationDefinition);
        }

        protected bool IsInstalled(Guid productGuid)
        {
            return Device.IsApplicationInstalled(productGuid);
        }

        public override InstallationResult Install(ApplicationDefinition applicationDefinition)
        {
            var windowsApplicationDefinition = ToWindowsPhoneApplicationDefinition(applicationDefinition);

            if (IsInstalled(windowsApplicationDefinition.ProductGuid))
                return InstallationResult.AlreadyInstalled;

            InvokeTrace("installing xap to device...");

            if (windowsApplicationDefinition.ProductGuid == Guid.Empty)
                throw new ArgumentException("Empty productId");

            if (!File.Exists(windowsApplicationDefinition.ApplicationPackagePath))
                throw new FileNotFoundException("File not found - " + windowsApplicationDefinition.ApplicationPackagePath);

            if (!File.Exists(windowsApplicationDefinition.ApplicationIconPath))
                throw new FileNotFoundException("File not found - " + windowsApplicationDefinition.ApplicationIconPath);

            Device.InstallApplication(
                windowsApplicationDefinition.ProductGuid,
                windowsApplicationDefinition.InstanceGuid,
                windowsApplicationDefinition.ApplicationName,
                windowsApplicationDefinition.ApplicationIconPath,
                windowsApplicationDefinition.ApplicationPackagePath);

            InvokeTrace("xap installed");
            return InstallationResult.Success;
        }

        public override UninstallationResult Uninstall(ApplicationDefinition applicationDefinition)
        {
            var windowsApplicationDefinition = ToWindowsPhoneApplicationDefinition(applicationDefinition);

            if (!Device.IsApplicationInstalled(windowsApplicationDefinition.ProductGuid))
                return UninstallationResult.NotInstalled;

            InvokeTrace("uninstalling xap from device...");
            var app = SafeGetApplication(windowsApplicationDefinition.ProductGuid);
            app.Uninstall();
            InvokeTrace("xap uninstalled from  device");

            return UninstallationResult.Success;
        }

        public override StopResult Stop(ApplicationDefinition applicationDefinition)
        {
            var windowsApplicationDefinition = ToWindowsPhoneApplicationDefinition(applicationDefinition);

            InvokeTrace("ensuring application is stopped...");

            if (!Device.IsApplicationInstalled(windowsApplicationDefinition.ProductGuid))
            {
                InvokeTrace("application is not installed");
                return StopResult.NotInstalled;
            }

            var app = SafeGetApplication(windowsApplicationDefinition.ProductGuid);
            if (app == null)
            {
                InvokeTrace("failed to get application");
                return StopResult.NotInstalled; // really this is an error case - but just return NotInstalled for now
            }

            /*
             IsRunning is not supported on WP7
            if (!app.IsRunning())
            {
                InvokeTrace("application is not running");
                return StopResult.NotRunning;
            }
             */

            InvokeTrace("stopping application...");
            app.TerminateRunningInstances();
            InvokeTrace("application stopped");
            return StopResult.Success;
        }

        private RemoteApplication SafeGetApplication(Guid productId)
        {
            try
            {
                var app = Device.GetApplication(productId);
                if (app == null)
                    throw new InvalidOperationException("Unexpected return - null app - sorry");
                return app;
            }
            catch (Exception exception)
            {
                InvokeTrace("Exception seen {0} - {1}", exception.GetType().FullName, exception.Message);
                return null;
            }
        }

        public override StartResult Start(ApplicationDefinition applicationDefinition)
        {
            var windowsApplicationDefinition = ToWindowsPhoneApplicationDefinition(applicationDefinition);

            InvokeTrace("launching app...");
            var app = SafeGetApplication(windowsApplicationDefinition.ProductGuid);
            if (app == null)
            {
                InvokeTrace("not installed");
                return StartResult.NotInstalled;
            }

            /*
             app.IsRunning is not supported for WP7
            if (app.IsRunning())
            {
                InvokeTrace("already running");
                return StartResult.AlreadyRunning;
            }
             */

            app.Launch();
            InvokeTrace("app launched");
            return StartResult.Success;
        }
    }
}