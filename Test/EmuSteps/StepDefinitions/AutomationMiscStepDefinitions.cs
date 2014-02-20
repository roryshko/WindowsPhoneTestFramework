//  ----------------------------------------------------------------------
//  <copyright file="AutomationMiscStepDefinitions.cs" company="Expensify">
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
using WindowsPhoneTestFramework.Server.Core;

namespace WindowsPhoneTestFramework.Test.EmuSteps.StepDefinitions
{
    [Binding]
    public class AutomationMiscStepDefinitions : EmuDefinitionBase
    {
        [StepDefinition(@"перейти (назад|вперед|домой|to start)")]
        [StepDefinition(@"I navigate (back|forward|home|to start)")]
        public void INavigate(string direction)
        {
            Assert.IsTrue(Emu.ApplicationAutomationController.Navigate(direction), "Unable to navigate {0}", direction);
        }

        [StepDefinition(@"установлен фокус на элемент ""([^\""]*)""")]
        [StepDefinition(@"I set focus to the control ""([^\""]*)""")]
        public void ThenISetFocusToTheControl(string control)
        {
            Assert.IsTrue(Emu.ApplicationAutomationController.SetFocus(control), "Unable to set focus to {0}", control);
        }

        [StepDefinition(@"приложение спит$")]
        [StepDefinition(@"I see my app is not running$")]
        public void AndMyAppIsNotRunning()
        {
            var result = Emu.ApplicationAutomationController.LookIsAlive();
            Assert.IsFalse(result, "App is still alive");
        }

        [StepDefinition("перейти на (.*)(panorama|pivot|item) в (left|right)")]
        [StepDefinition("I move the (.*)(panorama|pivot|item) (left|right)")]
        public void MovePivot(string named, string type, string direction)
        {
            var name = ControlName(named, type);

            var result = Emu.ApplicationAutomationController.Pivot(name,
                                                                   direction == "left" ? PivotType.Last : PivotType.Next);
            Assert.IsTrue(result, "Unable to move the {0} {1} {2}", named, type, direction);
        }

        // todo: [StepDefinition("перейти (.*)(panorama|pivot) на (?:the |)(.*)")]
        [StepDefinition("I move the (.*)(panorama|pivot) to (?:the |)(.*)")]
        public void MoveToNamedPivot(string named, string type, string item)
        {
            var name = ControlName(named, type);
            var itemName = ControlName(item, string.Empty);

            var controlFound = Emu.ApplicationAutomationController.WaitForControl(name);
            Assert.That(controlFound, "Unable to find the {0} {1}", named, type);

            var result = Emu.ApplicationAutomationController.Pivot(name, itemName);
            Assert.IsTrue(result, "Unable to move the {0} {1} to the {2}", named, type, item);
        }
    }
}