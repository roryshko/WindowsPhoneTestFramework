// ----------------------------------------------------------------------
// <copyright file="WindowsPhoneWindowsEmulatorDisplayInputController.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
//     
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using System.Drawing;
using WindowsInput;
using WindowsInput.Native;
using WindowsPhoneTestFramework.Server.Core.Tangibles;
using WindowsPhoneTestFramework.Server.DisplayInputControllerCore;
using NativeMethods = WindowsPhoneTestFramework.Server.DisplayInputControllerCore.NativeMethods;

namespace WindowsPhoneTestFramework.Server.WindowsPhoneDeviceController
{
    public class WindowsPhoneWindowsEmulatorDisplayInputController : WindowsEmulatorDisplayInputControllerBase
    {
        public WindowsPhoneWindowsEmulatorDisplayInputController()
            : base(new InputSimulator())
        {
            EmulatorWindowClassName = "XDE_LCDWindow";
            EmulatorWindowWindowName = string.Empty;

            //EmulatorSkinWindowWindowName = "Windows Phone Emulator";
            //EmulatorSkinWindowClassName = "XDE_SkinWindow";

            EmulatorProcessName = "XDE";
        }

        public override void EnsureHardwareKeyboardEnabled()
        {
            SendKeyPress(VirtualKeyCode.PRIOR);
        }

        public override void EnsureHardwareKeyboardDisabled()
        {
            InvokeTrace("Warning - EnsureHardwareKeyboardDisabled method is not currently operational - not sure why");
            SendKeyPress(VirtualKeyCode.NEXT);
        }

        protected override VirtualKeyCode HardwareButtonToKeyCode(PhoneHardwareButton whichHardwareButton)
        {
            VirtualKeyCode vk;
            switch (whichHardwareButton)
            {
                case PhoneHardwareButton.Back:
                    vk = VirtualKeyCode.F1;
                    break;
                case PhoneHardwareButton.Home:
                    vk = VirtualKeyCode.F2;
                    break;
                case PhoneHardwareButton.Search:
                    vk = VirtualKeyCode.F3;
                    break;
                case PhoneHardwareButton.Camera:
                    vk = VirtualKeyCode.F7;
                    break;
                case PhoneHardwareButton.VolumeUp:
                    vk = VirtualKeyCode.F9;
                    break;
                case PhoneHardwareButton.VolumeDown:
                    vk = VirtualKeyCode.F10;
                    break;
                case PhoneHardwareButton.Power:
                    vk = VirtualKeyCode.PRINT;
                    break;
                case PhoneHardwareButton.Menu:
                    // Menu Button is not supported on Windows Phone...
                    vk = VirtualKeyCode.NONAME;
                    break;

                default:
                    throw new ManipulationFailedException("Unknown Hardware Button " + whichHardwareButton);
            }

            return vk;
        }
   }
}