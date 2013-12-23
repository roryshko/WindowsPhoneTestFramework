//  ----------------------------------------------------------------------
//  <copyright file="AutomationWaitStepDefinitions.cs" company="Expensify">
//      (c) Copyright Expensify. http://www.expensify.com
//      This source is subject to the Microsoft Public License (Ms-PL)
//      Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//      All other rights reserved.
//  </copyright>
//  
//  Author - Stuart Lodge, Cirrious. http://www.cirrious.com
//  ------------------------------------------------------------------------

using System;
using System.Threading;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace WindowsPhoneTestFramework.Test.EmuSteps.StepDefinitions
{
    [Binding]
    public class AutomationWaitStepDefinitions : EmuDefinitionBase
    {
        [StepDefinition(@"ожидаем появления элемента ""([^\""]*)""$")]
        [StepDefinition(@"I wait for the control ""([^\""]*)"" to appear$")]
        public void ThenIWaitForControlToAppear(string controlId)
        {
            var result = Emu.ApplicationAutomationController.WaitForControl(controlId);
            Assert.IsTrue(result, "Failed to wait for control '{0}'", controlId);
        }

        [StepDefinition(@"ожидаем появления ([^\""]*)$")]
        [StepDefinition(@"I wait for the ([^\""]*) to appear$")]
        public void ThenIWaitForTheControlToAppear(string control)
        {
            var controlName = ControlName(control, string.Empty);
            var result = Emu.ApplicationAutomationController.WaitForControl(controlName);
            Assert.IsTrue(result, "Failed to wait for control '{0}'", controlName);
        }

        [StepDefinition(@"ожидаем появления текста ""([^\""]*)""$")]
        [StepDefinition(@"I wait for the text ""([^\""]*)"" to appear$")]
        public void ThenIWaitForTextToAppear(string text)
        {
            var result = Emu.ApplicationAutomationController.WaitForText(text);
            Assert.IsTrue(result, "Failed to wait for text '{0}'", text);
        }

        [StepDefinition(@"ждем (\d+\.?\d*) секунд$")]
        [StepDefinition(@"I wait (\d+\.?\d*) seconds$")]
        public void ThenIWait(double countSeconds)
        {
            Thread.Sleep(TimeSpan.FromSeconds(countSeconds));
        }

        [StepDefinition(@"ждем (\d+\.?\d*) минут$")]
        [StepDefinition(@"I wait (\d+\.?\d*) minutes$")]
        public void ThenIWaitMinutes(double minutes)
        {
            Thread.Sleep(TimeSpan.FromMinutes(minutes));
        }

        [StepDefinition(@"ждем 1 секунду$")]
        [StepDefinition(@"I wait 1 second$")]
        public void ThenIWait()
        {
            ThenIWait(1);
        }
    }
}