//  ----------------------------------------------------------------------
//  <copyright file="IDisplayInputController.cs" company="Expensify">
//      (c) Copyright Expensify. http://www.expensify.com
//      This source is subject to the Microsoft Public License (Ms-PL)
//      Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//      All other rights reserved.
//  </copyright>
//  
//  Author - Stuart Lodge, Cirrious. http://www.cirrious.com
//  ------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using WindowsPhoneTestFramework.Server.Core.Tangibles;
using WindowsPhoneTestFramework.Server.Utils;

namespace WindowsPhoneTestFramework.Server.Core
{
    public interface IDisplayInputController : ITrace
    {
        void EnsureWindowIsInForeground();
        void ReleaseWindowFromForeground();
        void EnsureHardwareKeyboardEnabled();
        void EnsureHardwareKeyboardDisabled();
        PhoneOrientation GuessOrientation();
        void PressHardwareButton(PhoneHardwareButton whichHardwareButton);
        void LongPressHardwareButton(PhoneHardwareButton whichHardwareButton);
        void LongPressHardwareButton(PhoneHardwareButton whichHardwareButton, TimeSpan duration);
        void DoGesture(IGesture gesture);
        void SendKeyPress(KeyboardKeyCode hardwareButtonToKeyCode);
        void SendKeyLongPress(KeyboardKeyCode virtualKeyCode, TimeSpan duration);
        void TextEntry(string text);
        void PerformMouseDownMoveUp(IEnumerable<Point> points, TimeSpan periodBetweenPoints);
        IEnumerable<Point> TranslatePhonePositionsToHostPositions(IEnumerable<Point> points);
    }
}