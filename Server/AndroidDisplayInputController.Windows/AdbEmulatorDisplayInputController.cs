//  ----------------------------------------------------------------------
//  <copyright file="AdbEmulatorDisplayInputController.cs" company="Expensify">
//      (c) Copyright Expensify. http://www.expensify.com
//      This source is subject to the Microsoft Public License (Ms-PL)
//      Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//      All other rights reserved.
//  </copyright>
//  
//  Author - Stuart Lodge, Cirrious. http://www.cirrious.com
//  ------------------------------------------------------------------------

using WindowsInput;
using WindowsInput.Native;
using WindowsPhoneTestFramework.Server.Core.Tangibles;
using WindowsPhoneTestFramework.Server.DisplayInputControllerCore;

// TODO - revisit this class with the telnet knowledge from http://stackoverflow.com/questions/1959012/how-can-i-unlock-the-screen-programmatically-in-android

namespace WindowsPhoneTestFramework.Server.AndroidDisplayInputController.Windows
{
    public interface IAndroidDisplayInputController
    {
        void EnsureScreenUnlocked();
        void SetEmulatorWindowName(string name);
    }

    public class AdbEmulatorDisplayInputController : WindowsEmulatorDisplayInputControllerBase,
                                                     IAndroidDisplayInputController
    {
        public AdbEmulatorDisplayInputController()
            : base(new InputSimulator())
        {
            EmulatorWindowClassName = "SDL_app";
            EmulatorWindowWindowName = "needs-to-be-inserted-by-controller";

            //EmulatorSkinWindowWindowName = "Windows Phone Emulator";
            //EmulatorSkinWindowClassName = "XDE_SkinWindow";

            EmulatorProcessName = "emulator-arm.exe";
        }

        public void EnsureScreenUnlocked()
        {
            // a "PageUp" key press seems to do the trick
            EnsureWindowIsInForeground();
            SendKeyPress(KeyboardKeyCode.PRIOR);
        }

        public void SetEmulatorWindowName(string name)
        {
            EmulatorWindowWindowName = name;
        }

        public override void EnsureHardwareKeyboardEnabled()
        {
            // nothing to do!
        }

        public override void EnsureHardwareKeyboardDisabled()
        {
            // nothing to do!
        }

        /*
        From: http://developer.android.com/guide/developing/tools/emulator.html#KeyMapping
         
        Implemented:
            Home	HOME
            Menu (left softkey)	F2 or Page-up button
            Back	ESC
            Search	F5
            Power button	F7
            Audio volume up button	KEYPAD_PLUS, Ctrl-5
            Audio volume down button	KEYPAD_MINUS, Ctrl-F6
            Camera button	Ctrl-KEYPAD_5, Ctrl-F3
         
        Not Implemented:
            Call/dial button	F3
            Hangup/end call button	F4
            Star (right softkey)	Shift-F2 or Page Down
            Switch to previous layout orientation (for example, portrait, landscape)	KEYPAD_7, Ctrl-F11
            Switch to next layout orientation (for example, portrait, landscape)	KEYPAD_9, Ctrl-F12
            Toggle cell networking on/off	F8
            Toggle code profiling	F9 (only with -trace startup option)
            Toggle fullscreen mode	Alt-Enter
            Toggle trackball mode	F6
            Enter trackball mode temporarily (while key is pressed)	Delete
            DPad left/up/right/down	KEYPAD_4/8/6/2
            DPad center click	KEYPAD_5
            Onion alpha increase/decrease	KEYPAD_MULTIPLY(*) / KEYPAD_DIVIDE(/)
         */

        protected override VirtualKeyCode HardwareButtonToKeyCode(PhoneHardwareButton whichHardwareButton)
        {
            VirtualKeyCode vk;
            switch (whichHardwareButton)
            {
                case PhoneHardwareButton.Back:
                    vk = VirtualKeyCode.ESCAPE;
                    break;
                case PhoneHardwareButton.Home:
                    vk = VirtualKeyCode.HOME;
                    break;
                case PhoneHardwareButton.Search:
                    vk = VirtualKeyCode.F5;
                    break;
                case PhoneHardwareButton.Camera:
                    // TODO - need CTRL also!
                    vk = VirtualKeyCode.F3;
                    break;
                case PhoneHardwareButton.VolumeUp:
                    vk = VirtualKeyCode.OEM_PLUS;
                    break;
                case PhoneHardwareButton.VolumeDown:
                    vk = VirtualKeyCode.OEM_MINUS;
                    break;
                case PhoneHardwareButton.Power:
                    vk = VirtualKeyCode.F7;
                    break;
                case PhoneHardwareButton.Menu:
                    vk = VirtualKeyCode.F2;
                    break;

                default:
                    throw new ManipulationFailedException("Unknown Hardware Button " + whichHardwareButton);
            }

            return vk;
        }
    }
}