// ----------------------------------------------------------------------
// <copyright file="AdbDeviceController.cs" company="Expensify">
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
using System.Linq;
using System.Threading;
using WindowsPhoneTestFramework.Server.AndroidDisplayInputController.Windows;
using WindowsPhoneTestFramework.Server.Core;
using WindowsPhoneTestFramework.Server.Core.Results;
using WindowsPhoneTestFramework.Server.DeviceController;

namespace WindowsPhoneTestFramework.Server.AndroidDeviceController
{
    // useful links
    // main answer in - http://stackoverflow.com/questions/2720164/android-process-killer
    // adb stuff - http://developer.android.com/guide/developing/tools/adb.html#shellcommands
    // emulator stuff - http://developer.android.com/guide/developing/tools/emulator.html
    // the way we identify the emulator - http://stackoverflow.com/questions/2214377/how-to-get-serial-number-or-id-of-android-emulator-after-it-runs
    // for window manip on osx, try: http://stackoverflow.com/questions/614185/window-move-and-resize-apis-in-os-x and http://stackoverflow.com/questions/2262516/getting-global-mouse-position-in-mac-os-x
    public class AdbDeviceController : DeviceControllerBase
    {
        private static readonly TimeSpan DoNotWait = TimeSpan.MinValue;
        private static readonly TimeSpan InfiniteWait = TimeSpan.MaxValue;
        private static readonly TimeSpan DefaultWait = TimeSpan.FromSeconds(10.0);
        private static readonly TimeSpan DefaultInstallWait =  TimeSpan.FromMinutes(2.0);
        private static readonly TimeSpan DefaultUninstallWait = TimeSpan.FromMinutes(2.0);
        private static readonly TimeSpan DefaultWaitForDeviceWait = TimeSpan.FromMinutes(3.0);
        private static readonly TimeSpan DefaultPauseAfterEmulatorComesOnline = TimeSpan.FromSeconds(30.0);
        
        private readonly AdbDeviceControllerConfiguration _configuration;

        public IAndroidDisplayInputController AndroidDisplayInputController { get { return DisplayInputController as IAndroidDisplayInputController; } }

        public AdbDeviceController(AdbDeviceControllerConfiguration configuration)
        {            
            _configuration = configuration;

            // for now hard code the windows emulator display input controller...
            DisplayInputController = new AdbEmulatorDisplayInputController();
        }

        public override bool TryConnect()
        {
            bool connect = TryConnectToEmulator();
            if (!connect)
                return false;

            InvokeTrace("Ensuring screen is unlocked...");
            AndroidDisplayInputController.SetEmulatorWindowName(_configuration.RunningEmulatorWindowName);
            AndroidDisplayInputController.EnsureScreenUnlocked();
            InvokeTrace("Screen unlock sent - emulator ready");

            return true;
        }

        private bool TryConnectToEmulator()
        {
            if (IsEmulatorRunning())
            {
                InvokeTrace("Emulator is running");
                return true;
            }

            // reset the Android Debug Bridge - just in case this helps
            // (because sometimes the ADB just loses track of connected devices)
            InvokeTrace("Emulator is not running - resetting adb...");
            ResetAdb();

            if (IsEmulatorRunning())
            {
                InvokeTrace("Emulator is running");
                return true;
            }

            InvokeTrace("Starting emulator '{0}'...", _configuration.RunningEmulatorWindowName);
            if (!StartNewEmulator())
            {
                InvokeTrace("Could not start emulator");
                return false;
            }

            InvokeTrace("Waiting for emulator...");
            try
            {
                var result = WaitForEmulatorToComeOnline();
                InvokeTrace("Wait completed: " + (result ? "success" : "failed"));
                if (!result)
                {
                    return false;
                }
                InvokeTrace("Pausing for {0} seconds - allow emulator to complete initialization", DefaultPauseAfterEmulatorComesOnline.TotalSeconds);
                Thread.Sleep(DefaultPauseAfterEmulatorComesOnline);
                return true;
            }
            catch (AutomationException automationException)
            {
                InvokeTrace("Wait failed: " + automationException.Message);
                return false;
            }
        }

        private bool WaitForEmulatorToComeOnline()
        {
            ExecuteAdb(DefaultWaitForDeviceWait, "-s {0} wait-for-device", _configuration.RunningEmulatorAdbName);
            return true;
        }

        public override void ReleaseDeviceConnection()
        {
            // nothing to do here
        }

        public override InstallationResult Install(ApplicationDefinition applicationDefinition)
        {
            var adbDefinition = new AdbApplicationDefinition(applicationDefinition);

            // first install the stub
            var installStubResult = Install(adbDefinition.StubPackagePath);
            if (installStubResult != InstallationResult.Success)
                InvokeTrace("TODO!");

            var installMainResult = Install(adbDefinition.PackagePath);
            return installMainResult;
        }

        public InstallationResult Install(string packagePath)
        {
            var results = ExecuteAdb(DefaultInstallWait, string.Format("-s {0} install {1}", _configuration.RunningEmulatorAdbName, packagePath));

            // observed return codes are:
            //    Failure [INSTALL_FAILED_ALREADY_EXISTS]
            //    Success

            if (results.Count == 0)
            {
                InvokeTrace("Empty response seen - assuming success");
                return InstallationResult.Success;
            }

            if (results.Any(x => x.Contains("INSTALL_FAILED_ALREADY_EXISTS")))
                return InstallationResult.AlreadyInstalled;

            if (results.Any(x => x.StartsWith("Success")))
                return InstallationResult.Success;

            throw new AutomationException(CreateErrorMessage("install", results));
        }

        private static string CreateErrorMessage(string actionName, IEnumerable<string> fullResults)
        {
            return string.Format("Failed to {0} - unknown why - full text:{1}", actionName, String.Join(";", fullResults));
        }

        public override UninstallationResult Uninstall(ApplicationDefinition applicationDefinition)
        {
            var adbDefinition = new AdbApplicationDefinition(applicationDefinition);

            // first uninstall the stub package
            Uninstall(adbDefinition.StubPackageName);

            // then uninstall the app package
            return Uninstall(adbDefinition.PackageName);
        }

        private UninstallationResult Uninstall(string packageName)
        {
            var results = ExecuteAdb(DefaultUninstallWait, "-s {0} uninstall {1}", _configuration.RunningEmulatorAdbName, packageName);

            if (results.Count == 0)
            {
                InvokeTrace("No response - so assuming uninstall succeeded");
                return UninstallationResult.Success;
            }

            if (results.Any(x => x.StartsWith("Success")))
                return UninstallationResult.Success;

            if (results.Any(x => x.StartsWith("Failure")))
                return UninstallationResult.NotInstalled;

            throw new AutomationException(CreateErrorMessage("uninstall", results));
        }

        public override StopResult Stop(ApplicationDefinition applicationDefinition)
        {
            // see - main answer in - http://stackoverflow.com/questions/2720164/android-process-killer
            InvokeTrace("Stop command ignored for Android");
            return StopResult.Success;
        }

        public override StartResult Start(ApplicationDefinition applicationDefinition)
        {
            var adbDefinition = new AdbApplicationDefinition(applicationDefinition);
            /*
            ExecuteAdb("-s {0} shell am start -a {1} -n {2}/{3}",
                            _configuration.RunningEmulatorAdbName,
                            adbDefinition.Action,
                            adbDefinition.PackageName,
                            adbDefinition.ActivityClassName);
             */

            // TODO - check return here?
            ExecuteAdb(DoNotWait,
                        "-s {0} shell am instrument -w {1}/android.test.InstrumentationTestRunner",
                        _configuration.RunningEmulatorAdbName,
                        adbDefinition.StubPackageName);

            InvokeTrace("Not currently checking the start response!");

            return StartResult.Success;
        }

        private void ResetAdb()
        {
            // the advice from Android seems to be to kill and restart the server occasionally
            ExecuteAdb("kill-server");
            ExecuteAdb("start-server");
        }

        private bool StartNewEmulator()
        {
            var result = ExecuteEmulator(string.Format("-no-boot-anim -ports {0},{1} @{2}",
                                              _configuration.ConsolePort,
                                              _configuration.AdbPort,
                                              _configuration.AvdName));

            return !(result.Any(line => line.StartsWith("PANIC")));
        }

        private bool IsEmulatorRunning()
        {
            InvokeTrace("Looking for android devices");
            var deviceTextLines = ExecuteAdb("devices");
            InvokeTrace("{0} devices found", deviceTextLines.Count());
            var emulatorLine = deviceTextLines.FirstOrDefault(x => x.StartsWith(_configuration.RunningEmulatorAdbName));
            if (emulatorLine == null)
                return false;

            bool isOnline = emulatorLine.EndsWith("device");
            InvokeTrace("{0} device found: {1}", isOnline ? "online" : "offline", emulatorLine);

            return isOnline;
        }

        private List<string> ExecuteAdb(string argumentsBase, params object[] argumentsParams)
        {
            return ExecuteAdb(DefaultWait, argumentsBase, argumentsParams);
        }

        private List<string> ExecuteAdb(TimeSpan timeout, string argumentsBase, params object[] argumentsParams)
        {
            return ExecuteExecutable(_configuration.AdbPath, timeout, argumentsBase, argumentsParams);
        }

        private List<string> ExecuteEmulator(string argumentsBase, params object[] argumentsParams)
        {
            return ExecuteExecutable(_configuration.EmulatorPath, InfiniteWait, argumentsBase, argumentsParams);
        }

        private List<string> ExecuteExecutable(string executablePath, TimeSpan timeout, string argumentsBase, params object[] argumentsParams)
        {
            var result = new List<string>();
            var processStartInfo = new ProcessStartInfo()
                                       {
                                           Arguments = string.Format(argumentsBase, argumentsParams),
                                           FileName = executablePath,
                                           CreateNoWindow = false,
                                           RedirectStandardInput = true,
                                           RedirectStandardError = true,
                                           RedirectStandardOutput = true,
                                           UseShellExecute = false,
                                       };
            var process = new Process()
                              {
                                  StartInfo = processStartInfo
                              };

            if (timeout != DoNotWait)
                process.OutputDataReceived += (sender, args) => result.Add(args.Data);

            InvokeTrace("Calling {0} with arguments '{1}'...", processStartInfo.FileName, processStartInfo.Arguments);
            process.Start();
            //InvokeTrace("Waiting for {0} to exit...", processStartInfo.FileName);
            if (timeout == DoNotWait)
            {
                InvokeTrace("Call made");
                return null;
            }

            if (!process.HasExited)
            {
                if (!process.WaitForExit(timeout == InfiniteWait ? int.MaxValue : (int) timeout.TotalMilliseconds))
                {
                    InvokeTrace("Wait timed out");
                    throw new AutomationException("Timeout failed while waiting for adb.exe to complete");
                }
            }

            if (!process.HasExited)
                InvokeTrace("Problem: call has not completed after wait");

            var message = string.Empty;
            if (result.Count > 0)
                message = string.Format("- {0} lines of output, starting '{1}'", result.Count, result.First());
            InvokeTrace("Call completed" + message);

            return result;
        }
    }
}
