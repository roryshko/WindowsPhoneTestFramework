//  ----------------------------------------------------------------------
//  <copyright file="AutomationPressStepDefinitions.cs" company="Expensify">
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
using WindowsPhoneTestFramework.Server.Core.Tangibles;

namespace WindowsPhoneTestFramework.Test.EmuSteps.StepDefinitions
{
    [Binding]
    public class AutomationPressStepDefinitions : EmuDefinitionBase
    {
        [StepDefinition(@"I press(?: the control | )""([^\""]*)""$")]
        public void ThenIPressTheNamedControl(string named)
        {
            Assert.IsTrue(Emu.ApplicationAutomationController.WaitForControl(named), "Failed to wait for control '{0}'",
                          named);
            Assert.IsTrue(Emu.ApplicationAutomationController.WaitForControlToBeEnabled(named),
                          "Failed to wait for control '{0}' to be enabled", named);
            Assert.IsTrue(Emu.ApplicationAutomationController.InvokeControlTapAction(named),
                          "Failed to press control '{0}'", named);
        }

        [StepDefinition("I press \"(.*)\" in the application bar")]
        public void PressTheTextInTheApplicationBar(string text)
        {
            LookForAppBarText(text, true);
            var result = Emu.ApplicationAutomationController.InvokeAppBarTap(text);
            Assert.That(result, Is.True, "Unable to press the application bar item with text \"{0}\"", text);
        }

        [StepDefinition("I (don't |)see \"(.*)\" in the application bar")]
        public void SeeTheTextInTheApplicationBar(string exists, string text)
        {
            var shouldSeeItem = exists.Trim().ToLowerInvariant() != "don't";
            LookForAppBarText(text, shouldSeeItem);
        }

        [StepDefinition("I toggle(?: the|) (.*)")]
        public void ToggleButton(string control)
        {
            var toggleControl = ControlName(control, "Toggle");
            Assert.That(Emu.ApplicationAutomationController.WaitForControl(toggleControl), Is.True,
                        "The {0} toggle control wasn't there", toggleControl);
            Thread.Sleep(500);
            Assert.That(Emu.ApplicationAutomationController.Toggle(toggleControl), Is.True, "Failed to toggle {0}",
                        toggleControl);
        }

        [StepDefinition(@"I hit return on the (\d.. |)([^,]*)(?:, in the |)(.*)$")]
        [StepDefinition(@"I press the (\d.. |)([^,""]*)(?:, in the |)([^""]*)$")]
        public void PressThe(int index, string itemname, string parentname)
        {
            itemname = ControlName(itemname, string.Empty);
            parentname = (! string.IsNullOrEmpty(parentname)) ? ControlName(parentname, string.Empty) : "";

            WaitForParent(parentname);

            Assert.IsTrue(Emu.ApplicationAutomationController.WaitForControl(itemname, index, parentname),
                          "Failed while waiting for element to press {0} of name '{1}', in {2}", index, itemname,
                          parentname);
            Assert.IsTrue(Emu.ApplicationAutomationController.WaitForControlToBeEnabled(itemname, index, parentname),
                          "Failed while waiting for element to press {0} of name '{1}', in {2} to be enabled", index,
                          itemname, parentname);
            Assert.IsTrue(Emu.ApplicationAutomationController.InvokeControlTapAction(itemname, index, parentname),
                          "Failed to press element {0} of name '{1}', in {2}", index, itemname, parentname);
        }

        [StepDefinition(@"I click the (.*) button")]
        public void ClickHardwareButton(PhoneHardwareButton button)
        {
            Thread.Sleep(TimeSpan.FromSeconds(3));

            Emu.DisplayInputController.PressHardwareButton(button);

            Thread.Sleep(TimeSpan.FromSeconds(5));
        }

        [StepDefinition("I acknowledge this message by selecting \"(.*)\"")]
        public void PressButtonInTheMessageBox(string buttonText)
        {
            var result = Emu.ApplicationAutomationController.InvokeMessageboxTapAction(buttonText);
            Assert.IsTrue(result, "Failed to press message box button '{0}'", buttonText);
        }

        [StepDefinition(@"I (continue|confirm|cancel) the message box")]
        public void IConfirmOrCancelTheMessageBox(string confirmOrCancel)
        {
            string button = "Left";
            if (confirmOrCancel.Equals("cancel", StringComparison.InvariantCultureIgnoreCase))
            {
                button = "Right";
            }

            And("I press the " + button + " button, in the message box");
        }

        private void LookForAppBarText(string text, bool shouldSeeItem)
        {
            var foundItem = Emu.ApplicationAutomationController.WaitForAppBarItem(text, TimeSpan.FromSeconds(10));
            Assert.That(
                shouldSeeItem,
                Is.EqualTo(foundItem),
                "{0} application bar item with text \"{1}\"",
                shouldSeeItem ? "Unable to find" : "Incorrectly found",
                text);
        }
    }
}