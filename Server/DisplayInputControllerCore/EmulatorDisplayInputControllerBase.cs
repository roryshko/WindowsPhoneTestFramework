// ----------------------------------------------------------------------
// <copyright file="EmulatorDisplayInputControllerBase.cs" company="Expensify">
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
using System.Drawing;
using System.Linq;
using System.Threading;
using WindowsInput;
using WindowsInput.Native;
using WindowsPhoneTestFramework.Server.Core;
using WindowsPhoneTestFramework.Server.Core.Tangibles;
using WindowsPhoneTestFramework.Server.Utils;

namespace WindowsPhoneTestFramework.Server.DisplayInputControllerCore
{
    public abstract class EmulatorDisplayInputControllerBase : TraceBase, IDisplayInputController
    {
        // constants - used for WindowsInput mouse positioning
        private const double VirtualScreenWidth = 65535.0;
        private const double VirtualScreenHeight = 65535.0;

        // constants - used for key code input
        private const VirtualKeyCode IgnoreThisKeyCode = VirtualKeyCode.NONAME;

        // pauses
        protected static readonly TimeSpan DefaultPauseDurationAfterAction = TimeSpan.FromMilliseconds(100.0);
        protected static readonly TimeSpan DefaultLongPressPauseDurationDuringAction = TimeSpan.FromMilliseconds(2000.0);
        public TimeSpan PauseDurationAfterSendingKeyPress { get; set; }
        public TimeSpan PauseDurationAfterSettingForegroundWindow { get; set; }
        public TimeSpan PauseDurationAfterPerformingGesture { get; set; }
        public TimeSpan PauseDurationAfterTextEntry { get; set; }

        // reference to input simulator from WindowsInput
        private readonly IInputSimulator _inputSimulator;

        protected EmulatorDisplayInputControllerBase(IInputSimulator inputSimulator)
        {
            _inputSimulator = inputSimulator;
            PauseDurationAfterSendingKeyPress = DefaultPauseDurationAfterAction;
            PauseDurationAfterSettingForegroundWindow = DefaultPauseDurationAfterAction;
            PauseDurationAfterPerformingGesture = DefaultPauseDurationAfterAction;
            PauseDurationAfterTextEntry = DefaultPauseDurationAfterAction;
        }

        public abstract void EnsureHardwareKeyboardEnabled();
        public abstract void EnsureHardwareKeyboardDisabled();
        protected abstract void SetEmulatorWindowInForeground();
        protected abstract void ReleaseEmulatorWindowFromForeground();
        protected abstract Rectangle GetEmulatorWindowRect();
        protected abstract VirtualKeyCode HardwareButtonToKeyCode(PhoneHardwareButton whichHardwareButton);

        public void EnsureWindowIsInForeground()
        {
            SetEmulatorWindowInForeground();
            Pause(PauseDurationAfterSettingForegroundWindow);
        }

        public void ReleaseWindowFromForeground()
        {
            ReleaseEmulatorWindowFromForeground();
            Pause(PauseDurationAfterSettingForegroundWindow);
        }

        public void PressHardwareButton(PhoneHardwareButton whichHardwareButton)
        {
            SendKeyPress(HardwareButtonToKeyCode(whichHardwareButton));
        }

        public void LongPressHardwareButton(PhoneHardwareButton whichHardwareButton)
        {
            LongPressHardwareButton(whichHardwareButton, DefaultLongPressPauseDurationDuringAction);
        }

        public void LongPressHardwareButton(PhoneHardwareButton whichHardwareButton, TimeSpan pressDuration)
        {
            SendKeyLongPress(HardwareButtonToKeyCode(whichHardwareButton), pressDuration);
        }

        public void DoGesture(IGesture gesture)
        {
            gesture.Perform(this);
            Pause(PauseDurationAfterPerformingGesture);
        }

        public void PerformMouseDownMoveUp(IEnumerable<Point> points, TimeSpan periodBetweenPoints)
        {
            // convert to array to ensure we don't perform too much linq
            var array = points.ToArray();

            if (array.Length < 2)
                throw new ManipulationFailedException("Requested PerformMouseDownMoveUp with too few points - {0}", array.Length);

            // mouse down at the start point
            var startPoint = array.First();
            _inputSimulator.Mouse.MoveMouseTo(startPoint.X, startPoint.Y);
            var lastMovedToPoint = startPoint;
            _inputSimulator.Mouse.LeftButtonDown();

            foreach (var point in array.Skip(1).Take(array.Length - 2))
            {
                _inputSimulator.Mouse.MoveMouseTo(point.X, point.Y);
                lastMovedToPoint = point;
                Pause(periodBetweenPoints);
            }

            var endPoint = array.Last();
            if (lastMovedToPoint.X != endPoint.X || lastMovedToPoint.Y != endPoint.Y)
                _inputSimulator.Mouse.MoveMouseTo(endPoint.X, endPoint.Y);

            _inputSimulator.Mouse.LeftButtonUp();
        }

        public PhoneOrientation GuessOrientation()
        {
            var rect = GetEmulatorWindowRect();
            return GuessOrientation(rect);
        }

        public Point TranslatePhonePositionToHostPosition(Point point)
        {
            var rect = GetEmulatorWindowRect();
            var screenRect = NativeMethods.GetDesktopRectangle();
            var emulatorScaleRatio = EstimateScaleRatio(rect);
            return new Point()
                       {
                           X = (int)(VirtualScreenWidth * (rect.X + emulatorScaleRatio * point.X) / screenRect.Width),
                           Y = (int)(VirtualScreenHeight * (rect.Y + emulatorScaleRatio * point.Y) / screenRect.Height)
                       };
        }

        public IEnumerable<Point> TranslatePhonePositionsToHostPositions(IEnumerable<Point> points)
        {
            var hostPoints = from p in points
                             select TranslatePhonePositionToHostPosition(p);
            return hostPoints;
        }

        private static double EstimateScaleRatio(Rectangle rect)
        {
            var orientation = GuessOrientation(rect);
            switch (orientation)
            {
                case PhoneOrientation.Landscape800By480:
                    return (double)rect.Width / 800.0;

                case PhoneOrientation.Portrait480By800:
                    return (double)rect.Width / 480.0;
            }

            throw new ManipulationFailedException("Unexpected orientation " + orientation);
        }

        private static PhoneOrientation GuessOrientation(Rectangle rect)
        {
            var ratio = ((double)rect.Width) / ((double)rect.Height);
            if (Math.Abs(ratio - 800.0 / 480.0) < 0.01)
                return PhoneOrientation.Landscape800By480;
            if (Math.Abs(ratio - 480.0 / 800.0) < 0.01)
                return PhoneOrientation.Portrait480By800;
            throw new ManipulationFailedException("Unable to guess ratio for width {0} height {1}", rect.Width, rect.Height);
        }

        public void SendKeyPress(KeyboardKeyCode keyboardKeyCode)
        {
            SendKeyPress(ConvertKeyCode(keyboardKeyCode));
        }

        public void SendKeyPress(VirtualKeyCode virtualKeyCode)
        {
            if (virtualKeyCode == IgnoreThisKeyCode)
                return;

            _inputSimulator.Keyboard.KeyPress(virtualKeyCode);
            Pause(PauseDurationAfterSendingKeyPress);
        }

        public void SendKeyLongPress(KeyboardKeyCode keyboardKeyCode, TimeSpan duration)
        {
            SendKeyLongPress(ConvertKeyCode(keyboardKeyCode), duration);            
        }

        public void SendKeyLongPress(VirtualKeyCode virtualKeyCode, TimeSpan duration)
        {
            if (virtualKeyCode == IgnoreThisKeyCode)
                return;

            _inputSimulator.Keyboard.KeyDown(virtualKeyCode);
            Pause(duration);
            _inputSimulator.Keyboard.KeyUp(virtualKeyCode);
            Pause(PauseDurationAfterSendingKeyPress);
        }

        public void TextEntry(string text)
        {
            InvokeTrace("Warning - TextEntry method is not currently operational - not sure why");
            _inputSimulator.Keyboard.TextEntry(text);
            Pause(PauseDurationAfterTextEntry);
        }

        private static VirtualKeyCode ConvertKeyCode(KeyboardKeyCode inputCode)
        {
            return (VirtualKeyCode)inputCode;
        }

        protected static void Pause(TimeSpan duration)
        {
            if (duration > TimeSpan.Zero)
                Thread.Sleep(duration);
        }
    }
}