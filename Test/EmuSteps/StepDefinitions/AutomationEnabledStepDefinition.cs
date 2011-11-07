using NUnit.Framework;
using TechTalk.SpecFlow;

namespace WindowsPhoneTestFramework.Test.EmuSteps.StepDefinitions
{
    [Binding]
    public class AutomationEnabledStepDefinition :EmuDefinitionBase
    {
        [Then(@"I see the controls are enabled")]
        public void ThenISeeTheNamedFieldsAreEnabled(Table table)
        {
            IterateOverNameTable(table, @"I see the control ""{0}"" is enabled");
        }

        [Then(@"I see the controls are disabled")]
        public void ThenISeeTheNamedFieldsAreDisabled(Table table)
        {
            IterateOverNameTable(table, @"I see the control ""{0}"" is disabled");
        }

        [Then(@"I see the control ""([^\""]*)"" is enabled")]
        public void ThenISeeTheNamedFieldIsEnabled(string namedField)
        {
            ThenISeeTheNamedFieldIsEnabled(namedField, true);
        }

        private void ThenISeeTheNamedFieldIsEnabled(string namedField, bool expectedIsEnabled)
        {
            bool actualIsEnabled;
            var result = Emu.ApplicationAutomationController.TryGetControlIsEnabled(namedField, out actualIsEnabled);
            Assert.IsTrue(result, "Failed to get enabled state for '{0}' - looking for '{1}'", actualIsEnabled, expectedIsEnabled);
            Assert.AreEqual(expectedIsEnabled, actualIsEnabled, "Enabeld state didn't match - field '{0}' - expected '{1}' - actual '{2}'", namedField, expectedIsEnabled, actualIsEnabled);
        }

        [Then(@"I see the control ""([^\""]*)"" is disabled")]
        public void ThenISeeTheNamedFieldIsDisabled(string namedField)
        {
            ThenISeeTheNamedFieldIsEnabled(namedField, false);
        }
    }
}