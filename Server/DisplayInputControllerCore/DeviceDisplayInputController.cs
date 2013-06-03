// ----------------------------------------------------------------------
// <copyright file="DisplayInputControllerBase.cs" company="Nokia">
//     (c) Copyright Nokia. http://www.nokia.com
//     All other rights reserved.
// </copyright>
// 
// Author - Ed Blacker, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using WindowsPhoneTestFramework.Server.Core;
using WindowsPhoneTestFramework.Server.Core.Tangibles;
using WindowsPhoneTestFramework.Server.Utils;

namespace WindowsPhoneTestFramework.Server.DisplayInputControllerCore
{
    public class DeviceDisplayInputController : TraceBase, IDisplayInputController
    {
        public Rectangle GetWindowRect()
        {
            // Todo: Feed this values from the actual device
            return new Rectangle(0, 0, 480, 800);
        }

        public PhoneOrientation GuessOrientation()
        {
            return PhoneOrientation.Portrait480By800;
        }

        public void EnsureWindowIsInForeground()
        {
            // Irrelevant to actual devices
        }

        public void ReleaseWindowFromForeground()
        {
            // Irrelevant to actual devices
        }

        public void EnsureHardwareKeyboardEnabled()
        {
            // Irrelevant to actual devices
        }

        public void EnsureHardwareKeyboardDisabled()
        {
            // Irrelevant to actual devices
        }

        public void PressHardwareButton(PhoneHardwareButton whichHardwareButton)
        {
            // Irrelevant to actual devices
        }

        public void LongPressHardwareButton(PhoneHardwareButton whichHardwareButton)
        {
            // Irrelevant to actual devices
        }

        public void LongPressHardwareButton(PhoneHardwareButton whichHardwareButton, TimeSpan duration)
        {
            // Irrelevant to actual devices
        }

        public void DoGesture(IGesture gesture)
        {
            // Irrelevant to actual devices
        }

        public void SendKeyPress(KeyboardKeyCode hardwareButtonToKeyCode)
        {
            // Irrelevant to actual devices
        }

        public void SendKeyLongPress(KeyboardKeyCode virtualKeyCode, TimeSpan duration)
        {
            // Irrelevant to actual devices
        }

        public void TextEntry(string text)
        {
            // Irrelevant to actual devices
        }

        public void PerformMouseDownMoveUp(IEnumerable<Point> points, TimeSpan periodBetweenPoints)
        {
            // Irrelevant to actual devices
        }

        public IEnumerable<Point> TranslatePhonePositionsToHostPositions(IEnumerable<Point> points)
        {
            // Irrelevant to actual devices
            return new List<Point>();
        }
    }
}