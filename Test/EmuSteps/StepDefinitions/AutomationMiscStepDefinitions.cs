// ----------------------------------------------------------------------
// <copyright file="AutomationMiscStepDefinitions.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using NUnit.Framework;
using TechTalk.SpecFlow;

namespace WindowsPhoneTestFramework.Test.EmuSteps.StepDefinitions
{
    [Binding]
    public class AutomationMiscStepDefinitions : EmuDefinitionBase
    {
        [Then(@"I set focus to the control ""([^\""]*)""")]
        public void ThenISetFocusToTheControl(string whichControl)
        {
            Assert.IsTrue(Emu.ApplicationAutomationController.SetFocus(whichControl));
        }

        [Then(@"I see my app is not running$")]
        public void AndMyAppIsNotRunning()
        {
            var result = Emu.ApplicationAutomationController.LookIsAlive();
            Assert.IsFalse(result, "App is still alive");
        }
    }
}