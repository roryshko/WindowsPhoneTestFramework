//  ----------------------------------------------------------------------
//  <copyright file="EmulatorWindowsPhoneDeviceController.cs" company="Expensify">
//      (c) Copyright Expensify. http://www.expensify.com
//      This source is subject to the Microsoft Public License (Ms-PL)
//      Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//      All other rights reserved.
//  </copyright>
//  
//  Author - Stuart Lodge, Cirrious. http://www.cirrious.com
//  ------------------------------------------------------------------------

namespace WindowsPhoneTestFramework.Server.WindowsPhoneDeviceController
{
    public class EmulatorWindowsPhoneDeviceController : WindowsPhoneDeviceControllerBase
    {
        public EmulatorWindowsPhoneDeviceController(string target)
            // this name should work for both English and non-English SDKs
            // e.g. Windows Phone Emulator
            // e.g. Windows Phone Emulator(DE)
            : base(target)
        {
            DisplayInputController = new WindowsPhoneWindowsEmulatorDisplayInputController();
        }
    }
}