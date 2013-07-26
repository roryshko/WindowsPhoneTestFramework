// ----------------------------------------------------------------------
// <copyright file="AutomationListStepDefinitions.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using NUnit.Framework;
using System;
using System.Threading;
using TechTalk.SpecFlow;
using WindowsPhoneTestFramework.Server.Core.Gestures;
using WindowsPhoneTestFramework.Server.Core.Tangibles;

namespace WindowsPhoneTestFramework.Test.EmuSteps.StepDefinitions
{
    [Binding]
    public class AutomationListStepDefinitions : EmuDefinitionBase
    {
        public AutomationListStepDefinitions()
        {
        }

#warning Investigate this commented out AutomationStepDefinitions ctor - can it be deleted?
        /*
        public AutomationStepDefinitions(IConfiguration configuration) : base(configuration)
        {
        }
         */
        [StepDefinition("I select the (\\d.. |)item in the (.*) list$")]
        public void ISelectTheNumberedItemInNamedList(int index, string control)
        {
            var controlName = ControlName(control, "List");
            var focusResult = Emu.ApplicationAutomationController.SetFocus(controlName);
            Assert.IsTrue(focusResult, "Failed to set focus to control '{0}'", controlName);
            var result = Emu.ApplicationAutomationController.SelectListItem(controlName, index);
            Assert.IsTrue(result, "Failed to select the {0} item in the control '{1}'", index, control);
        }

        [StepDefinition(@"I scroll ""([^\""]*)"" into view")] 
        public void IScrollTheGivenListitemIntoView(string listItem)
        {
            var result = Emu.ApplicationAutomationController.ScrollIntoViewListItem(listItem);
            Assert.IsTrue(result, "Failed to scroll \"{0}\" item into view", listItem);
            var focusResult = Emu.ApplicationAutomationController.SetFocus(listItem);
            Assert.IsTrue(focusResult, "Failed to set focus to control '{0}'", listItem);
        }
    }
}
