// ----------------------------------------------------------------------
// <copyright file="WindowsPhoneDriver.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------


using WindowsPhoneTestFramework.Server.DisplayInputControllerCore;
namespace WindowsPhoneTestFramework.Server.WindowsPhoneDeviceController
{
    public class PhoneWindowsPhoneDeviceController : WindowsPhoneDeviceControllerBase
    {
        public PhoneWindowsPhoneDeviceController()
            : base("Device")
        {
            DisplayInputController = new DeviceDisplayInputController();
        }

    }
}
