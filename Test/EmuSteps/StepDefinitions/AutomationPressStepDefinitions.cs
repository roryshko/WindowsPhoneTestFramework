// ----------------------------------------------------------------------
// <copyright file="AutomationPressStepDefinitions.cs" company="Expensify">
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
    public class AutomationPressStepDefinitions : EmuDefinitionBase
    {
        public AutomationPressStepDefinitions()
        {
        }

        /*
        public AutomationStepDefinitions(IConfiguration configuration) : base(configuration)
        {
        }
         */

        [Then(@"I press the control ""([^\""]*)""$")]
        public void ThenIPressTheNamedControl(string named)
        {
            var focusResult = Emu.ApplicationAutomationController.SetFocus(named);
            Assert.IsTrue(focusResult, "Failed to set focus to control '{0}'", named);
            var result = Emu.ApplicationAutomationController.InvokeControlTapAction(named);
            Assert.IsTrue(result, "Failed to press control '{0}'", named);
        }
    }
}
