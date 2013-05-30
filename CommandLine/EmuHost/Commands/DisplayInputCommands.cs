// ----------------------------------------------------------------------
// <copyright file="DisplayInputCommands.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using System;
using System.ComponentModel;
using WindowsPhoneTestFramework.CommandLine.CommandLineHost;
using WindowsPhoneTestFramework.Server.Core;
using WindowsPhoneTestFramework.Server.Core.Gestures;
using WindowsPhoneTestFramework.Server.Core.Tangibles;

namespace WindowsPhoneTestFramework.CommandLine.EmuHost.Commands
{
    public class DisplayInputCommands
    {
        public IDisplayInputController DisplayInputController { get; set; }

        [CommandLineCommand("hardwareButton")]
        [Description("press a hardware button - Back, Start, Search, Camera, VolumeUp, VolumeDown, Power - e.g. 'hardwareButton Back'")]
        public void PressHardware(string whichButton)
        {
            var parsedButton = (PhoneHardwareButton)Enum.Parse(typeof(PhoneHardwareButton), whichButton);
            DisplayInputController.EnsureWindowIsInForeground();
            DisplayInputController.EnsureHardwareKeyboardEnabled();
            DisplayInputController.PressHardwareButton(parsedButton);
            Console.WriteLine("hardwareButton: Completed");
        }

        [CommandLineCommand("textEntry")]
        [Description("enter text - e.g. 'enterText Hello World'")]
        public void TextEntry(string text)
        {
            DisplayInputController.EnsureWindowIsInForeground();
            DisplayInputController.EnsureHardwareKeyboardEnabled();
            DisplayInputController.TextEntry(text);
            Console.WriteLine("textEntry: Completed");
        }

        [CommandLineCommand("sendKeyPress")]
        [Description("enter a specific virtual key code - e.g. 'enterText VK_U'")]
        public void SendKeyPress(string whichCode)
        {
            var vk = (KeyboardKeyCode)Enum.Parse(typeof(KeyboardKeyCode), whichCode);
            DisplayInputController.EnsureWindowIsInForeground();
            DisplayInputController.EnsureHardwareKeyboardEnabled();
            DisplayInputController.SendKeyPress(vk);
            Console.WriteLine("sendKeyPress: Completed");
        }

        [CommandLineCommand("listKeyCodes")]
        [Description("lists all defined virtual key code - for info on key codes mapped for emulator, see http://msdn.microsoft.com/en-us/library/ff754352(v=VS.92).aspx - e.g. 'listKeyCodes'")]
        public void ListKeyCodes(string ignore)
        {
            foreach (var vk in Enum.GetValues(typeof(KeyboardKeyCode)))
            {
                Console.WriteLine(vk);
            }
            Console.WriteLine("listKeyCodes: Completed");
        }

        [CommandLineCommand("disableHardwareKeyboard")]
        [Description("disable the PC keyboard - the soft keyboard will then be available - note that other commands - textEntry, keyPress, hardwareButton - reset this as they use the PC keyboard buffers - e.g. 'disableHardwareKeyboard'")]
        public void DisableHardwareKeyboard(string ignored)
        {
            DisplayInputController.EnsureHardwareKeyboardDisabled();
            Console.WriteLine("disableHardwareKeyboard: Completed");
        }

        [CommandLineCommand("doSwipe")]
        [Description("obsolete - see doFlick instead")]
        public void SendSwipe(string whichSwipe)
        {
            Console.WriteLine("doSwipe has been renamed doFlick - try 'doFlick "  + whichSwipe + "'");
        }

        ////[CommandLineCommand("doFlick")]
        ////[Description("completes a flick gesture across the screen - currently only LeftToRight or RightToLeft across the horizontal and vertical middle of the screen supported - e.g. 'doFlick LeftToRight'")]
        ////public void SendFlick(string whichSwipe)
        ////{
        ////    var orientation = DisplayInputController.GuessOrientation();

        ////    var parsed = (FlickDirection)Enum.Parse(typeof(FlickDirection), whichSwipe);
        ////    IGesture gesture = null;
        ////    switch (parsed)
        ////    {
        ////        case FlickDirection.LeftToRight:
        ////            gesture = orientation == PhoneOrientation.Portrait480By800
        ////                          ? FlickGesture.LeftToRightPortrait()
        ////                          : FlickGesture.LeftToRightLandscape();
        ////            break;

        ////        case FlickDirection.RightToLeft:
        ////            gesture = orientation == PhoneOrientation.Portrait480By800
        ////                          ? FlickGesture.RightToLeftPortrait()
        ////                          : FlickGesture.RightToLeftLandscape();
        ////            break;

        ////        default:
        ////            throw new ArgumentException("Unexpected swipe " + parsed);
        ////    }

        ////    DisplayInputController.DoGesture(gesture);
        ////    Console.WriteLine("doFlick: Completed");
        ////}

        ////[CommandLineCommand("tapAt")]
        ////[Description("tap at the specifed position - e.g. 'tapAt 100,100'")]
        ////public void PressOnControl(string x_comma_y)
        ////{
        ////    var split = x_comma_y.Split(new char[] {','}, 2);
        ////    if (split.Length != 2)
        ////    {
        ////        Console.WriteLine("Input incorrect - need x,y");
        ////        return;
        ////    }

        ////    int x;
        ////    int y;
        ////    if (!int.TryParse(split[0], out x) || !int.TryParse(split[1], out y))
        ////    {
        ////        Console.WriteLine("Input incorrect - need integer x,y");
        ////        return;
        ////    }

        ////    IGesture gesture = TapGesture.TapOnPosition(x,y);
        ////    DisplayInputController.DoGesture(gesture);
        ////    Console.WriteLine("tapAt: Completed");
        ////}

        ////[CommandLineCommand("tapLongAt")]
        ////[Description("tap for specified time at the specifed position x,y,seconds - e.g. 'tapAt 100,100,2'")]
        ////public void LongPressOnControl(string x_comma_y)
        ////{
        ////    var split = x_comma_y.Split(new char[] { ',' }, 3);
        ////    if (split.Length != 3)
        ////    {
        ////        Console.WriteLine("Input incorrect - need x,y");
        ////        return;
        ////    }

        ////    int x;
        ////    int y;
        ////    if (!int.TryParse(split[0], out x) || !int.TryParse(split[1], out y))
        ////    {
        ////        Console.WriteLine("Input incorrect - need integer x,y");
        ////        return;
        ////    }

        ////    TapGesture gesture = new TapGesture(x, y, new TimeSpan(TimeSpan.TicksPerSecond * Convert.ToInt16(split[2])));
        ////    DisplayInputController.DoGesture(gesture);
        ////    Console.WriteLine("tapLongAt: Completed");
        ////}
    }
}