// ----------------------------------------------------------------------
// <copyright file="InputStepDefinitions.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using System;
using NUnit.Framework;
using TechTalk.SpecFlow;
using WindowsPhoneTestFramework.Server.Core;
using WindowsPhoneTestFramework.Server.Core.Gestures;
using WindowsPhoneTestFramework.Server.Core.Tangibles;

namespace WindowsPhoneTestFramework.Test.EmuSteps.StepDefinitions
{
    [Binding]
    public class InputStepDefinitions : EmuDefinitionBase
    {
        public InputStepDefinitions()
            : base()
        {
        }

        /*
        public InputStepDefinitions(IConfiguration configuration)
            : base(configuration)
        {
        }
        */

        [StepDefinition(@"������ ���������� ������ �����")]
        [StepDefinition(@"I go back")]
        public void ThenIGoBack()
        {
            Emu.DisplayInputController.PressHardwareButton(PhoneHardwareButton.Back);
        }

        [StepDefinition(@"������ ���������� ������ ����� (\d+) ������")]
        [StepDefinition(@"I press the back button for (\d+) seconds")]
        public void ThenILongPressBack(int timeInSeconds)
        {
            Emu.DisplayInputController.LongPressHardwareButton(PhoneHardwareButton.Back, TimeSpan.FromSeconds(timeInSeconds));
        }

        [StepDefinition(@"������ ���������� ������ �����")]
        [StepDefinition(@"I go home")]
        public void ThenIGoHome()
        {
            Emu.DisplayInputController.PressHardwareButton(PhoneHardwareButton.Home);
        }

        [StepDefinition(@"������ ���������� ������ ""([^\""]*)""$")]
        public void ThenIPressHardwareButton(string whichButton)
        {
            switch (whichButton)
            {
                case "�����":
                    whichButton = "Back";
                    break;
                case "�����":
                    whichButton = "Home";
                    break;
                case "�����":
                    whichButton = "Search";
                    break;
                case "������":
                    whichButton = "Camera";
                    break;
                case "��������� ���������":
                    whichButton = "VolumeUp";
                    break;
                case "��������� ���������":
                    whichButton = "VolumeDown";
                    break;
                case "�������":
                    whichButton = "Power";
                    break;
                case "����": // � wp �����������
                    whichButton = "Menu";
                    break;
            }

            PhoneHardwareButton parsedButton;
            Assert.IsTrue(Enum.TryParse(whichButton, true, out parsedButton), "failed to parse button name " + whichButton);
            Emu.DisplayInputController.PressHardwareButton(parsedButton);
        }

        // /^I press "([^\"]*)"$/

        // /^I press button number (\d+)$/

        // /^I press the "([^\"]*)" button$/

        // /^I press view with name "([^\"]*)"$/

        // /^I press image button number (\d+)$/

        // /^I press list item number (\d+)$/

        // /^I toggle checkbox number (\d+)$/



        // /^I enter "([^\"]*)" as "([^\"]*)"$/

        // /^I enter "([^\"]*)" into "([^\"]*)"$/

        // /^I enter "([^\"]*)" into input field number (\d+)$/

        // /^I clear "([^\"]*)"$/

        // /^I clear input field number (\d+)$/

        // /^I wait for "([^\"]*)" to appear$/

        // /^I wait for (\d+) seconds$/

        // /^I wait for dialog to close$/

        // /^I wait for progress$/

        // /^I wait for the "([^\"]*)" button to appear$/

        // /^I wait$/

        // /^I go back$/

#warning Tap and Flick methods removed from InputStepDefinitions - can these be reinstated?

        /*

        [Then(@"I flick ""([^\""]*)""$")]
        public void ThenIFlick(string flickDirection)
        {
            IGesture gesture = null;
            switch (flickDirection)
            {
                case "LeftToRight":
                    gesture = FlickGesture.LeftToRightPortrait();
                    break;
                case "RightToLeft":
                    gesture = FlickGesture.RightToLeftPortrait();
                    break;
                default:
                    Assert.Fail("Unknown flick " + flickDirection);
                    break;
            }
 
            Emu.DisplayInputController.DoGesture(gesture);
        }
 
        [Then(@"I tap on the middle of the screen")]
        public void ThenITapTheCenterOfTheScreen() 
        {
            ThenITapOnPercentPosition(50, 50);
        }

        private static int PercentToPosition(int percentage, int oneHundredPercentValue)
        {
            return (int) (percentage*oneHundredPercentValue/100.0);
        }

        [Then(@"/^I tap on screen (\d+)% from the left and (\d+)% from the top$/")]
        public void ThenITapOnPercentPosition(int xPercent, int yPercent)
        {
            var orientation = Emu.DisplayInputController.GuessOrientation();
            var screenSize = orientation.ScreenSize();
            ThenITapOnPosition(PercentToPosition(xPercent, screenSize.Width), PercentToPosition(yPercent, screenSize.Height));
        }

        [Then(@"/^I tap on screen (\d+) from the left and (\d+) from the top$/")]
        public void ThenITapOnPosition(int x, int y)
        {
            IGesture gesture = TapGesture.TapOnPosition(x, y);
            Emu.DisplayInputController.DoGesture(gesture);
        } 
         */
    }
}