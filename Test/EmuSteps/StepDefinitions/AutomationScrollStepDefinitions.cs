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
using System;
using System.Threading;
using TechTalk.SpecFlow;
using WindowsPhoneTestFramework.Server.Core.Tangibles;

namespace WindowsPhoneTestFramework.Test.EmuSteps.StepDefinitions
{
    [Binding]
    public class AutomationScrollStepDefinitions : EmuDefinitionBase
    {
        [StepDefinition(@"I scroll to the (\d.. |)([^,]*)(?:, in the |)(.*)$")]
        public void ScrollToThe(int index, string itemname, string parentname)
        {
            itemname = ControlName(itemname, string.Empty);
            parentname = (! string.IsNullOrEmpty(parentname)) ? ControlName(parentname, string.Empty) : "";

            WaitForParent(parentname);

            Assert.IsTrue(Emu.ApplicationAutomationController.WaitForControl(itemname, index, parentname), "Failed while waiting for element to press {0} of name '{1}', in {2}", index, itemname, parentname);
            Thread.Sleep(500);
            Assert.IsTrue(Emu.ApplicationAutomationController.ScrollIntoView(itemname, index, parentname), "Failed to scroll to element {0} of name '{1}', in {2}", index, itemname, parentname);
            Thread.Sleep(1000);
        }
    }
}
