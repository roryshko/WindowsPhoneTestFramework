// ----------------------------------------------------------------------
// <copyright file="AutomationSeeStepDefinitions.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using System.Drawing;
using NUnit.Framework;
using TechTalk.SpecFlow;
using WindowsPhoneTestFramework.Server.Core;
using WindowsPhoneTestFramework.Server.Core.Gestures;
using WindowsPhoneTestFramework.Server.Core.Tangibles;

namespace WindowsPhoneTestFramework.Test.EmuSteps.StepDefinitions
{
    [Binding]
    public class InputWithAutomationStepDefinitions : AutomationPositionStepDefinitionsBase
    {
        [Then(@"I tap in the middle of the control ""([^\""]*)""$")]
        public void ThenITapTheMiddleOfTheControl(string controlId)
        {
            var position = Emu.ApplicationAutomationController.GetPositionOfControlOrText(controlId);

            var gesture = TapGesture.TapOnPosition(
                (int) (position.Left + position.Width/2),
                (int) (position.Top + position.Height/2));
            Emu.DisplayInputController.DoGesture(gesture);
        }
    }
}