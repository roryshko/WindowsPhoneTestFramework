using System.Drawing;
using NUnit.Framework;
using TechTalk.SpecFlow;
using WindowsPhoneTestFramework.Server.Core.Tangibles;

namespace WindowsPhoneTestFramework.Test.EmuSteps.StepDefinitions
{
    [Binding]
    public class AutomationSeeStepDefinitions : EmuDefinitionBase
    {
        public AutomationSeeStepDefinitions()
        {
        }

        [Then(@"I see the values$")]
        public void ThenISeeTheNamedFieldWithContent(Table table)
        {
            IterateOverNameValueTable(table, (@"I see the control ""{0}"" contains ""{1}"""));
        }

        [Then(@"I see the control ""([^\""]*)"" contains ""([^\""]*)""$")]
        public void ThenISeeTheNamedFieldWithContent(string namedField, string expectedContents)
        {
            string actualContents;
            var result = Emu.ApplicationAutomationController.TryGetTextFromControl(namedField, out actualContents);
            Assert.IsTrue(result, "Failed to get field contents for '{0}' - looking for '{1}'", namedField, expectedContents);
            Assert.AreEqual(expectedContents, actualContents, "Contents didn't match - field '{0}' - expected '{1}' - actual '{2}'", namedField, expectedContents, actualContents);
        }

        [Then(@"I see the control ""([^\""]*)"" has value ""([^\""]*)""$")]
        public void ThenISeeTheNamedFieldWithValue(string namedField, string expectedValue)
        {
            string actualValue;
            var result = Emu.ApplicationAutomationController.TryGetValueFromControl(namedField, out actualValue);
            Assert.IsTrue(result, "Failed to get field contents for '{0}' - looking for '{1}'", namedField, expectedValue);
            Assert.AreEqual(expectedValue, actualValue, "Contents didn't match - field '{0}' - expected '{1}' - actual '{2}'", namedField, expectedValue, actualValue);
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

        [Then(@"I see the control ""([^\""]*)"" is left of the control ""([^\""]*)""$")]
        public void ThenISeeControlOnTheLeftOfControl(string leftControlId, string rightControlId)
        {
            var leftPosition = Emu.ApplicationAutomationController.GetPositionOfControlOrText(leftControlId);
            var rightPosition = Emu.ApplicationAutomationController.GetPositionOfControlOrText(rightControlId);
            Assert.Less(leftPosition.X, rightPosition.X);
            Assert.LessOrEqual(leftPosition.X + leftPosition.Width, rightPosition.X);
        }

        #region Helper methods

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

        #endregion
    }
}