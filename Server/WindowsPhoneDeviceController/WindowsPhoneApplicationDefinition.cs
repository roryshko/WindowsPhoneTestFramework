// ----------------------------------------------------------------------
// <copyright file="WindowsPhoneApplicationDefinition.cs" company="Expensify">
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
using System.Text;
using WindowsPhoneTestFramework.Server.Core;
using WindowsPhoneTestFramework.Server.DeviceController;

namespace WindowsPhoneTestFramework.Server.WindowsPhoneDeviceController
{
    public class WindowsPhoneApplicationDefinition : ParsedApplicationDefinition
    {
        public override string ParsePrefix { get { return "WindowsPhone."; } }

        public string ApplicationId { get; set; }
        public string ApplicationName { get; set; }
        public string ApplicationPackagePath { get; set; }
        public string ApplicationIconPath { get; set; }

        public Guid ProductGuid { get { return SafeParseGuid(ApplicationId); } }
        public Guid InstanceGuid { get { return SafeParseGuid(ApplicationId); } }

        public WindowsPhoneApplicationDefinition(ApplicationDefinition applicationDefinition)
            : base(applicationDefinition)
        {
        }

        private static Guid SafeParseGuid(string input)
        {
            Guid toReturn;
            if (Guid.TryParse(input, out toReturn))
                return toReturn;

            return Guid.Empty;
        }
    }
}
