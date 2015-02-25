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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
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
            WpSdkName = "Windows Phone ";
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
                // Return the cached device
                if (_device != null)
                    return _device;

                // Get CoreCon DataStore
                InvokeTrace("creating datastore");
                var dsmgrObj = new DatastoreManager(1033);

                // Get the plaform
                InvokeTrace("getting platform");
                var platforms = dsmgrObj.GetPlatforms();
                InvokeTrace("{0} platform(s) found", platforms.Count);
                Platform phoneSdk = platforms.Single(p => p.Name.Contains(WpSdkName));
                InvokeTrace("platform '{0}' found", phoneSdk.Name);

                // find the device
                InvokeTrace(string.Format("looking for device '{0}'", WpDeviceNameBase));
                var devices = phoneSdk.GetDevices();
                InvokeTrace(string.Format("{0} devices found", (object) devices.Count));
                var device = devices.FirstOrDefault(d => d.Name == WpDeviceNameBase);

                if (device == null)
                {
                    InvokeTrace("device {0} not found - looking for similar matches", WpDeviceNameBase);
                    device = devices.FirstOrDefault(d => d.Name.Contains(WpDeviceNameBase));
                }

                if (device == null)
                {
                    InvokeTrace("device {0} not found - and no similar matches found", WpDeviceNameBase);
                    InvokeTrace("available devices were", WpDeviceNameBase);
                    foreach (var d in devices)
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

#warning Need to explain why GC.Collect(); is called here - any time this is called my spider sense screams
            GC.Collect();
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

#warning Can we kill this IsRunning block - it's commented out and really dead now?
            /*
             IsRunning is not supported on WP7
            if (!app.IsRunning())
            {
                InvokeTrace("application is not running");
                return StopResult.NotRunning;
            }
             */

            InvokeTrace("stopping application...");
            try
            {
                app.TerminateRunningInstances();
            }
            catch (SmartDeviceException ex)
            {
                InvokeTrace("failed to stopping application (SmartDeviceException ex) {0}", ex.ErrorCode, ex.Message, ex.ToString() );
                InvokeTrace("SmartDeviceException {0}, {1}", ex.ErrorCode, ex.Message );
                InvokeTrace("> {0}", ex.ToString() );
                return StopResult.FailToStop;
            }
            catch (Exception ex)
            {
                // for windows phone 8 emulator killing process only close window, 
                // but virtual machine stay running
                InvokeTrace("failed to stopping application (Exception ex)");
#if false
#warning Nested pokemon exception handling - hard to read and understand
                try
                {
                    InvokeTrace("CurrentDeviceID =" + Device.Id);
                    // An exception here can't be recovered but we can leave things in a better state for the next test if we kill the emulator
                    var processes = Process.GetProcessesByName("Xde");
                    foreach (var process in processes)
                    {
                        InvokeTrace("ProcessId = " + process.Id);
                        process.Kill();
                    }
                }
                catch (Exception)
                {
                    // We can but try, but if we can't let's ignore it
                    InvokeTrace("failed to close emulator");
                }
#endif
                throw ex;
            }

            InvokeTrace("application stopped");
            return StopResult.Success;
        }

        public override bool ForceDeviceShutDown()
        {
            try
            {
                var processes = Process.GetProcessesByName("Xde");

                // As this actually calls a whole load of code lets just get it
                var device = Device;

                InvokeTrace("Shutting down: " + device.Name);

                foreach (var process in processes)
                {
                    // Only kill the process we are on
                    if (process.MainWindowTitle == device.Name)
                    {
                        device.Disconnect();
                        InvokeTrace("Killing: " + process.Id);
                        process.Kill();
                    }
                }

                _device = null; //This allows the next call process to re-get the device
            }
            catch (Exception)
            {
                // We can but try, but if we can't let's ignore it
                InvokeTrace("failed to close emulator");
                return false;
            }

            return true;
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

        public override string GetIsolatedStorage(ApplicationDefinition applicationDefinition)
        {
            var store = GetIsoStorage(applicationDefinition);

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\IsolatedStore\Temp\" + Guid.NewGuid().ToString(); 
            ReceiveDirectory(store, desktopPath);

            return desktopPath;
        }

        protected RemoteIsolatedStorageFile GetIsoStorage(ApplicationDefinition applicationDefinition)
        {
            var windowsApplicationDefinition = ToWindowsPhoneApplicationDefinition(applicationDefinition);

            var app = SafeGetApplication(windowsApplicationDefinition.ProductGuid);

            var store = app.GetIsolatedStore();
            return store;
        }

        public override void RestoreIsolatedStorage(ApplicationDefinition applicationDefinition, string isolatedStorage)
        {
            var store = GetIsoStorage(applicationDefinition);

            if (Directory.Exists(isolatedStorage))
            {
                Console.WriteLine("Restoring isolated storage from: {0}", isolatedStorage);

                SendDirectory(store, isolatedStorage);
            }
            else
            {
                Console.WriteLine("Isolated storage folder \"{0}\" does not exist.", isolatedStorage);
            }
        }

        #region Lifted from ISETool

        private static void ReceiveDirectory(RemoteIsolatedStorageFile risf, string desktopDirPath)
        {
            if (Path.DirectorySeparatorChar != desktopDirPath[desktopDirPath.Length - 1])
            {
                desktopDirPath = desktopDirPath + (object)Path.DirectorySeparatorChar;
            }

            desktopDirPath = desktopDirPath + "IsolatedStore";
            var target = new DirectoryInfo(desktopDirPath);
            try
            {
                List<RemoteFileInfo> directoryListing = risf.GetDirectoryListing(string.Empty);
                ReceiveDeviceDirectory(risf, string.Empty, directoryListing, target, true);
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("Info: IsolatedStore for this application is empty.");
                throw ex;
            }
        }

        private static void ReceiveDeviceDirectory(RemoteIsolatedStorageFile risf, string deviceRelativeDir, List<RemoteFileInfo> source, DirectoryInfo target, bool cleanDeviceDirectory)
        {
            if (cleanDeviceDirectory)
            {
                string fullName = target.FullName;
                try
                {
                    if (Directory.Exists(fullName))
                    {
                        target.Delete(true);
                        target = Directory.CreateDirectory(fullName);
                    }
                }
                catch (IOException ex)
                {
                    Console.WriteLine("Error: Unable to create folder on desktop.");
                    throw ex;
                }
            }

            foreach (RemoteFileInfo remoteFileInfo in source)
            {
                if (remoteFileInfo.IsDirectory())
                {
                    string fileName = Path.GetFileName(remoteFileInfo.Name);
                    string str = deviceRelativeDir + (object)Path.DirectorySeparatorChar + fileName;
                    DirectoryInfo subdirectory = target.CreateSubdirectory(fileName);
                    
                    try
                    {
                        List<RemoteFileInfo> directoryListing = risf.GetDirectoryListing(str);
                        ReceiveDeviceDirectory(risf, str, directoryListing, subdirectory, false);
                    }
                    catch (FileNotFoundException ex)
                    {
                    }
                }
                else
                {
                    string fileName = Path.GetFileName(remoteFileInfo.Name);
                    string sourceDeviceFilePath = deviceRelativeDir + (object)Path.DirectorySeparatorChar + fileName;
                    string targetDesktopFilePath = target.FullName + (object)Path.DirectorySeparatorChar + fileName;
                    risf.ReceiveFile(sourceDeviceFilePath, targetDesktopFilePath, true);
                }
            }
        }

        private static void SendDirectory(RemoteIsolatedStorageFile risf, string desktopDirPath)
        {
            desktopDirPath = Path.GetFullPath(desktopDirPath);
            
            if (!Directory.Exists(desktopDirPath))
            {
                Console.WriteLine("Error: Path '{0}' does not exist.", (object)desktopDirPath);
                throw new DirectoryNotFoundException();
            }
            else
            {
                DirectoryInfo desktopDirInfo = new DirectoryInfo(desktopDirPath);
                SendDesktopDirectory(risf, desktopDirInfo, string.Empty, true);
            }
        }

        private static void SendDesktopDirectory(RemoteIsolatedStorageFile risf, DirectoryInfo desktopDirInfo, string deviceDirPath, bool cleanDeviceDirectory)
        {
            if (cleanDeviceDirectory)
            {
                try
                {
                    risf.DeleteDirectory(deviceDirPath);
                    risf.CreateDirectory(deviceDirPath);
                }
                catch (Exception ex)
                {
                }
            }
            
            foreach (FileInfo fileInfo in desktopDirInfo.GetFiles())
            {
                risf.SendFile(fileInfo.FullName, Path.Combine(deviceDirPath, fileInfo.Name), true);
            }

            foreach (DirectoryInfo desktopDirInfo1 in desktopDirInfo.GetDirectories())
            {
                string str = Path.Combine(deviceDirPath, desktopDirInfo1.Name);
                risf.CreateDirectory(str);
                SendDesktopDirectory(risf, desktopDirInfo1, str, false);
            }
        }

        #endregion

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