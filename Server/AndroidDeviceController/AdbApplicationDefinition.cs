// ----------------------------------------------------------------------
// <copyright file="AdbApplicationDefinition.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using WindowsPhoneTestFramework.Server.Core;
using WindowsPhoneTestFramework.Server.DeviceController;

namespace WindowsPhoneTestFramework.Server.AndroidDeviceController
{
    public class AdbApplicationDefinition : ParsedApplicationDefinition
    {
        public override string ParsePrefix { get { return "Android."; } }

        public string Action { get; set; }
        public string ActivityClassName { get; set; }
        public string PackagePath { get; set; }
        public string PackageName { get; set; }
        public string StubPackagePath { get; set; }
        public string StubPackageName { get; set; }

        public AdbApplicationDefinition(ApplicationDefinition applicationDefinition)
            : base(applicationDefinition)
        {
        }
    }
}
