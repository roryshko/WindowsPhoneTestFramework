// ----------------------------------------------------------------------
// <copyright file="AutomationEnterStepDefinitions.cs" company="Expensify">
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
    public class AutomationEnterStepDefinitions : EmuDefinitionBase
    {
        public AutomationEnterStepDefinitions()
        {
        }

        // TODO - this doesn't quite match the LessPainful platform... as this replaces the contents...
        [Then(@"I enter ""([^\""]*)"" into the control ""([^\""]*)""$")]
        public void ThenIEnterTextIntoTheNamedField(string contents, string namedField)
        {
            var result = Emu.ApplicationAutomationController.SetTextOnControl(namedField, contents);
            Assert.IsTrue(result, "Failed to enter text into '{0}'", namedField);
        }

        [Then(@"I set the value of the control ""([^\""]*)"" to ""([^\""]*)""$")]
        public void ThenISetTheValueOfTheNamedField(string namedField, string value)
        {
            var result = Emu.ApplicationAutomationController.SetValueOnControl(namedField, value);
            Assert.IsTrue(result, "Failed to set value on '{0}'", namedField);
        }
    }
}