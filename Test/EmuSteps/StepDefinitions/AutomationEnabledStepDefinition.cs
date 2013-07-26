//  ----------------------------------------------------------------------
//  <copyright file="AutomationEnabledStepDefinition.cs" company="Expensify">
//      (c) Copyright Expensify. http://www.expensify.com
//      This source is subject to the Microsoft Public License (Ms-PL)
//      Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//      All other rights reserved.
//  </copyright>
//  
//  Author - Stuart Lodge, Cirrious. http://www.cirrious.com
//  ------------------------------------------------------------------------

using NUnit.Framework;
using TechTalk.SpecFlow;

namespace WindowsPhoneTestFramework.Test.EmuSteps.StepDefinitions
{
    [Binding]
    public class AutomationEnabledStepDefinition : EmuDefinitionBase
    {
        [StepDefinition(@"I see the controls are enabled")]
        public void ThenISeeTheNamedFieldsAreEnabled(Table table)
        {
            IterateOverNameTable(table, @"I see the {0} is enabled");
        }

        [StepDefinition(@"I see the controls are disabled")]
        public void ThenISeeTheNamedFieldsAreDisabled(Table table)
        {
            IterateOverNameTable(table, @"I see the {0} is disabled");
        }

        [StepDefinition(@"I see the ([^\""]*) is (enabled|disabled)")]
        public void ThenISeeTheNamedFieldIsEnabled(string namedField, string enabled)
        {
            var expectedIsEnabled = enabled == "enabled";
            ThenISeeTheNamedFieldIsEnabled(namedField, expectedIsEnabled);
        }

        [StepDefinition(@"I wait for the ([^\""]*) to be enabled")]
        public void ThenIWaitForTheControlToBeEnabled(string namedField)
        {
            namedField = ControlName(namedField, string.Empty);

            Emu.ApplicationAutomationController.WaitForControl(namedField);
            var result = Emu.ApplicationAutomationController.WaitForControlToBeEnabled(namedField);

            Assert.IsTrue(result, "Control wasn't enabled by the timeout");
        }

        private void ThenISeeTheNamedFieldIsEnabled(string namedField, bool expectedIsEnabled)
        {
            bool actualIsEnabled;
            namedField = ControlName(namedField, string.Empty);

            Emu.ApplicationAutomationController.WaitForControl(namedField);
            var result = Emu.ApplicationAutomationController.TryGetControlIsEnabled(namedField, out actualIsEnabled);

            Assert.IsTrue(result, "Failed to get enabled state for '{0}' - looking for '{1}'", actualIsEnabled,
                          expectedIsEnabled);
            Assert.AreEqual(
                expectedIsEnabled,
                actualIsEnabled,
                "Enabeld state didn't match - field '{0}' - expected '{1}' - actual '{2}'",
                namedField,
                expectedIsEnabled,
                actualIsEnabled);
        }
    }
}