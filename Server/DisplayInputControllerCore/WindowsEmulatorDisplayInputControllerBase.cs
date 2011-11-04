// ----------------------------------------------------------------------
// <copyright file="WindowsEmulatorDisplayInputControllerBase.cs" company="Expensify">
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

namespace WindowsPhoneTestFramework.Server.DisplayInputControllerCore
{
    public abstract class WindowsEmulatorDisplayInputControllerBase : EmulatorDisplayInputControllerBase 
    {
        // Emulator Win32 Windows process name, window class names and window titles
        // - these are made public in case your environment uses different class/window names
        public string EmulatorWindowClassName { get; set; }
        public string EmulatorWindowWindowName { get; set; }
        public string EmulatorProcessName { get; set; }

        protected WindowsEmulatorDisplayInputControllerBase(IInputSimulator inputSimulator)
            : base(inputSimulator)
        {            
        }

        protected override void SetEmulatorWindowInForeground()
        {
            /*
             * this code left over from attempt to use the skin window - the attempt failed
             *
             
            public string EmulatorSkinWindowClassName { get; set; }
            public string EmulatorSkinWindowWindowName { get; set; }
             
            var result = NativeMethods.EnsureWindowIsInForeground(EmulatorSkinWindowClassName, EmulatorSkinWindowClassName);
            if (!result)
                throw new ManipulationFailedException("Failed to bring emulator skin window to foreground");
             
            */

            var topMostResult = NativeMethods.MakeWindowTopMost(EmulatorWindowClassName, EmulatorWindowWindowName);
            if (!topMostResult)
                throw new ManipulationFailedException("Failed to bring emulator skin window to topMost");

            var result = NativeMethods.EnsureWindowIsInForeground(EmulatorWindowClassName, EmulatorWindowWindowName);
            if (!result)
                throw new ManipulationFailedException("Failed to bring emulator window to foreground");
        }

        protected override void ReleaseEmulatorWindowFromForeground()
        {
            if (!NativeMethods.RevokeWindowTopMost(EmulatorProcessName))
                InvokeTrace("Failed to revoke emulator window topmost"); // this is ignored for now... no need to throw an exception here
        }

        protected override Rectangle GetEmulatorWindowRect()
        {
            var rect = NativeMethods.GetWindowRectangle(EmulatorWindowClassName, EmulatorWindowWindowName);
            if (rect.IsEmpty)
                throw new ManipulationFailedException("Failed to get emulator window rectangle");
            return rect;
        }

    }
}