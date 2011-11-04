// ----------------------------------------------------------------------
// <copyright file="AdbDeviceControllerConfiguration.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using System.Linq;
using System.Collections.Generic;
using System.IO;
using WindowsPhoneTestFramework.Server.Core;
using WindowsPhoneTestFramework.Server.DeviceController;

namespace WindowsPhoneTestFramework.Server.AndroidDeviceController
{
    public class AdbDeviceControllerConfiguration : ParsedObject
    {
        private const string DefaultConsolePort = "23001";
        private const string DefaultAdbPort = "23002";

        public override string ParsePrefix
        {
            get { return "Android."; }
        }

        public AdbDeviceControllerConfiguration(IDictionary<string, string> fields)
            : base(fields)
        {
            if (string.IsNullOrEmpty(ConsolePort))
                ConsolePort = DefaultConsolePort;
            if (string.IsNullOrEmpty(AdbPort))
                AdbPort = DefaultAdbPort;

            var nonFilledStringProperyList = string.Join(";", NonFilledStringPropertyNames());
            if (!string.IsNullOrEmpty(nonFilledStringProperyList))
                throw new AutomationException("Missing Android configuration fields: " + nonFilledStringProperyList);
        }

        public string AvdName { get; set; }
        public string ConsolePort { get; set; }
        public string AdbPort { get; set; }
        public string SdkPath { get; set; }

        //public uint ConsolePort { get { return uint.Parse(ConsolePort); } }
        //public uint AdbPort { get { return uint.Parse(AdbPort); } }

        public string AdbPath { get { return Path.Combine(SdkPath, "platform-tools", "adb"); } }
        public string EmulatorPath { get { return Path.Combine(SdkPath, "tools", "emulator"); } }
        public string RunningEmulatorAdbName { get { return "emulator-" + ConsolePort; } }
        public string RunningEmulatorWindowName { get { return string.Format("{0}:{1}", ConsolePort, AvdName); } }
    }
}