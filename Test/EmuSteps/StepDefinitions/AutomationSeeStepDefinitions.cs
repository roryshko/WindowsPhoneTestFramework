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

using NUnit.Framework;
using System;
using System.Threading;
using TechTalk.SpecFlow;

namespace WindowsPhoneTestFramework.Test.EmuSteps.StepDefinitions
{
    [Binding]
    public class AutomationSeeStepDefinitions : AutomationPositionStepDefinitionsBase
    {
        [StepDefinition(@"I see the values$")]
        public void ThenISeeTheNamedFieldWithContent(Table table)
        {
            IterateOverNameValueTable(table, (@"I see the control ""{0}"" contains ""{1}"""));
        }

        [StepDefinition(@"I see the controls")]
        public void ThenISeeTheControls(Table table)
        {
            IterateOverNameTable(table, @"I see the control ""{0}""");
        }

        [StepDefinition(@"I don't see the controls")]
        public void ThenIDoNotSeeTheControls(Table table)
        {
            IterateOverNameTable(table, ThenIDontSeeControl);
        }

        [StepDefinition(@"I see the control ""([^\""]*)"" contains ""([^\""]*)""$")]
        public void ThenISeeTheNamedFieldWithContent(string namedField, string expectedContents)
        {
            string actualContents;
            var result = Emu.ApplicationAutomationController.TryGetTextFromControl(namedField, out actualContents);
            Assert.IsTrue(result, "Failed to get field contents for '{0}' - looking for '{1}'", namedField, expectedContents);
            Assert.AreEqual(expectedContents, actualContents, "Contents didn't match - field '{0}' - expected '{1}' - actual '{2}'", namedField, expectedContents, actualContents);
        }

        [StepDefinition(@"I see the control ""([^\""]*)"" has value ""([^\""]*)""$")]
        public void ThenISeeTheNamedFieldWithValue(string namedField, string expectedValue)
        {
            string actualValue;
            Assert.IsTrue(Emu.ApplicationAutomationController.WaitForControlOrText(namedField), "Failed waiting for control named {0} to appear", namedField);
            var result = Emu.ApplicationAutomationController.TryGetValueFromControl(namedField, out actualValue);
            Assert.IsTrue(result, "Failed to get field contents for '{0}' - looking for '{1}'", namedField, expectedValue);
            Assert.AreEqual(expectedValue, actualValue, "Contents didn't match - field '{0}' - expected '{1}' - actual '{2}'", namedField, expectedValue, actualValue);
        }

        [StepDefinition(@"I see ""([^\""]*)""$")]
        public void ThenISee(string textOrControlId)
        {
            var position = Emu.ApplicationAutomationController.GetPositionOfControlOrText(textOrControlId);
            AssertPositionIsVisible(position, textOrControlId);
        }

        [StepDefinition(@"I may see the text ""([^\""]*)""$")]
        public void ThenIMaySeeText(string contents)
        {
            var position = Emu.ApplicationAutomationController.GetPositionOfText(contents);
            var seen = IsPositionVisible(position);

            StepFlowOutputHelpers.Write(
                seen ? "I saw the optional text '{0}'" : "I didn't see the optional text '{0}'", contents);
        }

        [StepDefinition(@"I see the text ""([^\""]*)""$")]
        public void ThenISeeText(string contents)
        {
            Emu.ApplicationAutomationController.WaitForControlOrText(contents, TimeSpan.FromSeconds(30));
            var position = Emu.ApplicationAutomationController.GetPositionOfText(contents);
            AssertPositionIsVisible(position, contents);
        }

        [StepDefinition(@"I don't see the text ""([^\""]*)""$")]
        public void ThenIDontSeeText(string contents)
        {
            var position = Emu.ApplicationAutomationController.GetPositionOfText(contents);
            AssertPositionIsNotVisible(position, contents);
        }

        [StepDefinition(@"I see the control ""([^\""]*)""$")]
        public void ThenISeeControl(string controlId)
        {
            Assert.True(IsControlVisible(controlId), "control not visible {0}", controlId);
        }

        [StepDefinition(@"I don't see the control ""([^\""]*)""$")]
        public void ThenIDontSeeControl(string controlId)
        {
            Assert.False(IsControlVisible(controlId), "control is visible {0}", controlId);
        }

        [StepDefinition(@"I see the control ""([^\""]*)"" is left of the control ""([^\""]*)""$")]
        public void ThenISeeControlOnTheLeftOfControl(string leftControlId, string rightControlId)
        {
            var leftPosition = Emu.ApplicationAutomationController.GetPositionOfControlOrText(leftControlId);
            var rightPosition = Emu.ApplicationAutomationController.GetPositionOfControlOrText(rightControlId);
            Assert.Less(leftPosition.X, rightPosition.X);
            Assert.LessOrEqual(leftPosition.X + leftPosition.Width, rightPosition.X);
        }

        [StepDefinition("I see (\\d) (.*) (item|button)(?:s*)")]
        [StepDefinition("I see at least (\\d) (.*) (item|button)(?:s*)")]
        public void SeeANumberOfItems(int count, string named, string type)
        {
            Emu.ApplicationAutomationController.WaitForControl(named, TimeSpan.FromSeconds(10));

            for (var index = 0; index < count; index++)
            {
                var controlName = ControlName(named, type);
                var setFocus = Emu.ApplicationAutomationController.SetFocus(controlName, index);
                if (!setFocus)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(2));
                    setFocus = Emu.ApplicationAutomationController.SetFocus(controlName, index);
                }

                Assert.IsTrue(setFocus, "Failed to set focus to control '{0}'", new object[] { controlName });
            }
        }

        [StepDefinition("I see the ([^\\\"]*) (panorama|pivot|list|item|button|link|box|bar|scrollviewer|panel|text|title|page)")]
        public void ThenISeeThe(string named, string type)
        {
            var controlName = ControlName(named, type);
            Assert.IsTrue(Emu.ApplicationAutomationController.WaitForControl(controlName), "The control '{0}' is not on the page", new object[] { controlName });
            Assert.True(IsControlVisible(controlName), "The control '{0}' is not visible ", controlName);
        }

        [StepDefinition("I do not see the ([^\\\"]*) (panorama|pivot|list|item|button|link|box|bar|scrollviewer|panel|text|title|NotificationButton)")]
        public void ThenIDoNotSeeThe(string named, string type)
        {
            var controlName = ControlName(named, type);
            var result = Emu.ApplicationAutomationController.WaitForControl(controlName);
            if (result)
            {
                Assert.False(IsControlVisible(controlName), "The control '{0}' is incorrectly visible ", controlName);
            }
        }

        [StepDefinition("I wait to see the text \"(.*)\"")]
        public void SeeTheText(string text)
        {
            var textAvailable = Emu.ApplicationAutomationController.LookForText(text);
            if (!textAvailable)
            {
                Thread.Sleep(TimeSpan.FromSeconds(2));
                textAvailable = Emu.ApplicationAutomationController.LookForText(text);
            }

            Assert.IsTrue(textAvailable, "Did not see the text {0} after waiting 2 seconds", text);
        }

        [StepDefinition("I wait until the text \"(.*)\" (disappears|appears) for a max of (\\d{1,3}) seconds")]
        public void WaitUntilTheTextAppears(string text, string appearance, int maxSeconds)
        {
            var startedAt = DateTime.Now;
            var success = appearance.Equals("appears", StringComparison.InvariantCultureIgnoreCase)
                              ? Emu.ApplicationAutomationController.WaitForText(text, TimeSpan.FromSeconds(maxSeconds))
                              : Emu.ApplicationAutomationController.WaitForTextToDisappear(
                                  text, TimeSpan.FromSeconds(maxSeconds));

            Assert.IsTrue(success, "Did not see the text {0} {1} after waiting {2} seconds", text, appearance, DateTime.Now.Subtract(startedAt).TotalSeconds);
        }

        [StepDefinition("I see the text \"([^\"]*)\" on the (\\d.. |)([^,]*)(?:, within the |)(.*)$")]
        public void SeeButtonWithText(string text, int ordinal, string control, string parentName)
        {
            var controlName = ControlName(control, string.Empty);
            string controlText;
            Thread.Sleep(TimeSpan.FromSeconds(1));

            parentName = (!string.IsNullOrEmpty(parentName)) ? ControlName(parentName, string.Empty) : "";

            if (!string.IsNullOrEmpty(parentName))
            {
                Assert.IsTrue(Emu.ApplicationAutomationController.WaitForControl(controlName, 0, parentName),
                              "The control '{0}' within the parent {2} is not on the page", new object[] {controlName});
            }

            Emu.ApplicationAutomationController.TryGetTextFromControl(controlName, out controlText, ordinal, parentName);
            Assert.AreEqual(text, controlText);
        }

        [StepDefinition("I see (.*) progress on (?:the|)([^\\\"]*)")]
        public void SeeSomeProgress(string progress, string control)
        {
            var controlName = ControlName(control, string.Empty);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            var progressValues = Emu.ApplicationAutomationController.GetProgressOfControl(controlName);
            if (progress == "some")
            {
                Assert.Greater(progressValues.Current, 0, "There was no progress on " + controlName);
            }
            else
            {
                var progressVal = double.Parse(progress);
                Assert.Greater(progressValues.Current, progressVal, "There was not enough progress on " + controlName);
            }
        }

        [StepDefinition("I (do not |)see a message box with \"([^\\\"]*)\" title")]
        public void SeeAMessageBoxWithTheTitle(string exists, string title)
        {
            var doNotSee = exists.Trim().ToLowerInvariant() == "do not";
            var result = Emu.ApplicationAutomationController.WaitForMessageBox(title, null, null);
            if (doNotSee)
            {
                Assert.IsFalse(result, "A message box with the title '{0}' was incorrectly found", title);
            }
            else
            {
                Assert.IsTrue(result, "A message box with the title '{0}' was not found", title);
            }
        }

        [StepDefinition("I see a message box with \"([^\\\"]*)\" title and \"([^\\\"]*)\" button")]
        public void SeeAMessageBoxWithTheTitleAndButton(string title, string button)
        {
            var buttons = new[] { button };
            var result = Emu.ApplicationAutomationController.WaitForMessageBox(title, null, buttons);

            Assert.IsTrue(result, "A message box with the title '{0}' and button '{1}' was not found", title, button);
        }

        [StepDefinition("I see a message box with \"([^\\\"]*)\" title and \"([^\\\"]*)\", \"([^\\\"]*)\" buttons")]
        public void SeeAMessageBoxWithTheTitleAndButtons(string title, string button1, string button2)
        {
            var buttons = new[] { button1, button2 };
            var result = Emu.ApplicationAutomationController.WaitForMessageBox(title, null, buttons);

            Assert.IsTrue(result, "A message box with the title '{0}' and buttons '{1} {2}' was not found", title, button1, button2);
        }
    }
}