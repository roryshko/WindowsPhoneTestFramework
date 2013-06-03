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

        [StepDefinition(@"I go back")]
        public void ThenIGoBack()
        {
            Emu.DisplayInputController.PressHardwareButton(PhoneHardwareButton.Back);
        }

        [StepDefinition(@"I press the back button for (\d+) seconds")]
        public void ThenILongPressBack(int timeInSeconds)
        {
            Emu.DisplayInputController.LongPressHardwareButton(PhoneHardwareButton.Back, TimeSpan.FromSeconds(timeInSeconds));
        }

        [StepDefinition(@"I go home")]
        public void ThenIGoHome()
        {
            Emu.DisplayInputController.PressHardwareButton(PhoneHardwareButton.Home);
        }

        [StepDefinition(@"I press hardware button ""([^\""]*)""$")]
        public void ThenIPressHardwareButton(string whichButton)
        {
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
    }
}