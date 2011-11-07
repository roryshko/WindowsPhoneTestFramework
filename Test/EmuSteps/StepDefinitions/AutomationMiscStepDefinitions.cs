using NUnit.Framework;
using TechTalk.SpecFlow;

namespace WindowsPhoneTestFramework.Test.EmuSteps.StepDefinitions
{
    [Binding]
    public class AutomationMiscStepDefinitions : EmuDefinitionBase
    {
        [Then(@"I see my app is not running$")]
        public void AndMyAppIsNotRunning()
        {
            var result = Emu.ApplicationAutomationController.LookIsAlive();
            Assert.IsFalse(result, "App is still alive");
        }
    }
}