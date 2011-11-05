// ----------------------------------------------------------------------
// <copyright file="AutomationStepDefinitions.cs" company="Expensify">
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
using WindowsPhoneTestFramework.Server.Core.Tangibles;

namespace WindowsPhoneTestFramework.Server.EmuSteps.StepDefinitions
{
    [Binding]
    public class AutomationStepDefinitions : EmuDefinitionBase
    {
        public AutomationStepDefinitions()
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
            var result = Emu.ApplicationAutomationController.InvokeControlTapAction(named);
            Assert.IsTrue(result, "Failed to click control '{0}'", named);
        }

        [Then(@"I see the control ""([^\""]*)"" contains ""([^\""]*)""$")]
        public void ThenISeeTheNamedFieldWithContent(string namedField, string expectedContents)
        {
            string actualContents;
            var result = Emu.ApplicationAutomationController.TryGetTextFromControl(namedField, out actualContents);
            Assert.IsTrue(result, "Failed to get field contents for '{0}' - looking for '{1}'", namedField, expectedContents);
            Assert.AreEqual(expectedContents, actualContents, "Contents didn't match - field '{0}' - expected '{1}' - actual '{2}'", namedField, expectedContents, actualContents);
        }

        // TODO - this doesn't quite match the LessPainful platform... as this replaces the contents...
        [Then(@"I enter ""([^\""]*)"" into the control ""([^\""]*)""$")]
        public void ThenIEnterTextIntoTheNamedField(string contents, string namedField)
        {
            var result = Emu.ApplicationAutomationController.SetTextOnControl(namedField, contents);
            Assert.IsTrue(result, "Failed to enter text into '{0}'", namedField);
        }

        [Then(@"I see ""([^\""]*)""$")]
        public void ThenISee(string textOrControlId)
        {
            var position = Emu.ApplicationAutomationController.GetPositionOfControlOrText(textOrControlId);
            AssertPositionIsVisible(position, textOrControlId);
        }

        [Then(@"I may see the text ""([^\""]*)""$")]
        public void ThenIMaySeeText(string contents)
        {
            var seen = false;

            var position = Emu.ApplicationAutomationController.GetPositionOfText(contents);
            seen = IsPositionVisible(position);

            if (seen)
                StepFlowOutputHelpers.Write("I saw the optional text '{0}'", contents);
            else
                StepFlowOutputHelpers.Write("I didn't see the optional text '{0}'", contents);                           
        }

        [Then(@"I see the text ""([^\""]*)""$")]
        public void ThenISeeText(string contents)
        {
            var position = Emu.ApplicationAutomationController.GetPositionOfText(contents);
            AssertPositionIsVisible(position, contents);
        }

        [Then(@"I don't see the text ""([^\""]*)""$")]
        public void ThenIDontSeeText(string contents)
        {
            var position = Emu.ApplicationAutomationController.GetPositionOfText(contents);
            AssertPositionIsNotVisible(position, contents);
        }

        [Then(@"I see the control ""([^\""]*)""$")]
        public void ThenISeeControl(string controlId)
        {
            Assert.True(IsControlVisible(controlId), "control not visible {0}", controlId);
        }

        [Then(@"I don't see the control ""([^\""]*)""$")]
        public void ThenIDontSeeControl(string controlId)
        {
            Assert.False(IsControlVisible(controlId), "control is visible {0}", controlId);
        }

        [Then(@"I see my app is not running$")]
        public void AndMyAppIsNotRunning()
        {
            var result = Emu.ApplicationAutomationController.LookIsAlive();
            Assert.IsFalse(result, "App is still alive");
        }

        [Then(@"I see the control ""([^\""]*)"" is left of the control ""([^\""]*)""$")]
        public void ThenISeeControlOnTheLeftOfControl(string leftControlId, string rightControlId)
        {
            var leftPosition = Emu.ApplicationAutomationController.GetPositionOfControlOrText(leftControlId);
            var rightPosition = Emu.ApplicationAutomationController.GetPositionOfControlOrText(rightControlId);
            Assert.Less(leftPosition.X, rightPosition.X);
            Assert.LessOrEqual(leftPosition.X + leftPosition.Width, rightPosition.X);
        }

        private bool IsControlVisible(string controlId)
        {
            var position = Emu.ApplicationAutomationController.GetPositionOfControl(controlId);
            return IsPositionVisible(position);
        }

        private bool IsPositionVisible(RectangleF position)
        {
            if (position.IsEmpty)
                return false;

            var phoneOrientation = Emu.DisplayInputController.GuessOrientation();
            return position.IsVisible(phoneOrientation);
        }

        private void AssertPositionIsNotVisible(RectangleF position, string textTest)
        {
            Assert.False(IsPositionVisible(position), "Position of is offscreen, text:'{0}', position:{1}", textTest, position);            
        }

        private void AssertPositionIsVisible(RectangleF position, string textTest)
        {
            Assert.True(IsPositionVisible(position), "Position of is offscreen, text:'{0}', position:{1}", textTest, position);
        }
    }
}
