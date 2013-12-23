//  ----------------------------------------------------------------------
//  <copyright file="AutomationEnterStepDefinitions.cs" company="Expensify">
//      (c) Copyright Expensify. http://www.expensify.com
//      This source is subject to the Microsoft Public License (Ms-PL)
//      Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//      All other rights reserved.
//  </copyright>
//  
//  Author - Stuart Lodge, Cirrious. http://www.cirrious.com
//  ------------------------------------------------------------------------

using System;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace WindowsPhoneTestFramework.Test.EmuSteps.StepDefinitions
{
    /// <summary>
    /// Should have a $ on the regexes to end them, but specflow is being a bit weird and
    /// matching them as a literal, not as part of the regex...
    /// </summary>
    [Binding]
    public class AutomationEnterStepDefinitions : EmuDefinitionBase
    {
        // TODO - this doesn't quite match the LessPainful platform... as this replaces the contents...
        [StepDefinition(@"вводим ""([^\""]*)"" в элемент ""([^\""]*)""")]
        [StepDefinition(@"I enter ""([^\""]*)"" into the control ""([^\""]*)""")]
        public void ThenIEnterTextIntoTheNamedField(string contents, string namedField)
        {
            var result = Emu.ApplicationAutomationController.SetTextOnControl(namedField, contents);
            Assert.IsTrue(result, "Failed to enter text into '{0}'", namedField);
        }

        [StepDefinition(@"установить значение элемента ""([^\""]*)"" в ""([^\""]*)""")]
        [StepDefinition(@"I set the value of the control ""([^\""]*)"" to ""([^\""]*)""")]
        public void ThenISetTheValueOfTheNamedField(string namedField, string value)
        {
            var result = Emu.ApplicationAutomationController.SetValueOnControl(namedField, value);
            Assert.IsTrue(result, "Failed to set value on '{0}'", namedField);
        }

        [StepDefinition(@"вводим следующие значения")]
        [StepDefinition(@"I enter values")]
        public void StepIEnterTheValues(Table table)
        {
            IterateOverNameValueTable(table, (@"I enter ""{1}"" into the control ""{0}"""));
        }

        [StepDefinition("ввести \"([^\\\"]*)\" в (?!элемент )([^,]*)")]
        [StepDefinition("I enter \"([^\\\"]*)\" (?:into|in to) the (?!control )([^,]*)")]
        [StepDefinition("ввести ([^\\\"]*) в (?!элемент )([^,]*)")]
        [StepDefinition("I enter ([^\\\"]*) (?:into|in to) the (?!control )([^,]*)")]
        public void IEnterTextAndHitReturn(string text, string control)
        {
            var controlName = ControlName(control, string.Empty);

            Emu.ApplicationAutomationController.WaitForControl(controlName, TimeSpan.FromSeconds(30));

            Emu.ApplicationAutomationController.WaitForControlToBeEnabled(controlName, timeout: TimeSpan.FromSeconds(30));

            Emu.ApplicationAutomationController.SetFocus(controlName);

            Assert.True(Emu.ApplicationAutomationController.SetTextOnControl(controlName, text));
        }
    }
}