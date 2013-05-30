// ----------------------------------------------------------------------
// <copyright file="IApplicationAutomationController.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;

namespace WindowsPhoneTestFramework.Server.Core
{
    public interface IApplicationAutomationController
    {
        #region Public Methods and Operators

        bool ControlContainsImage(string controlId, string imageName, int ordinal = 0, string parentId = null);

        bool FindMessageBox(string title, string message, string[] buttons);

        string GetControlColor(string controlIdOrText, int ordinal = 0, string parentId = null);

        RectangleF GetPositionOfControl(string controlId, int ordinal = 0, string parentId = null);

        RectangleF GetPositionOfControlOrText(string textOrControlId, int ordinal = 0, string parentId = null);

        RectangleF GetPositionOfText(string text);

        ProgressValues GetProgressOfControl(string controlId);

        ProgressValues GetProgressOfControlOrText(string textOrControlId);

        ProgressValues GetProgressOfText(string text);

        bool HorizontalScroll(string controlId, int amount);

        bool InvokeAppBarTap(string text);

        bool InvokeControlTapAction(string controlId, int ordinal = 0, string parentId = null);

        bool InvokeMessageboxTapAction(string buttonText);

        bool LookForAppBarItem(string text);

        bool LookForControl(string controlId, int ordinal = 0, string parentId = null);

        bool LookForText(string text);

        bool LookIsAlive();

        bool Navigate(string direction);

        bool Pivot(string pivotName, PivotType pivot);

        bool Pivot(string pivotName, string itemName);

        bool ScrollIntoView(string controlId, int ordinal = 0, string parentId = null);

        bool ScrollIntoViewListItem(string controlWithinItemId);

        bool SelectListItem(string selectorName, int indexOfItemToSelect);

        bool SelectListItem(string controlWithinItemId);

        bool SetApplicationSettings(string key, string value);

        bool SetApplicationSettings(Dictionary<string, string> values);

        bool SetFocus(string controlId, int ordinal = 0, string parentId = null);

        bool SetTextOnControl(string controlId, string text, int ordinal = 0, string parentId = null);

        bool SetValueOnControl(string controlId, string value, int ordinal = 0, string parentId = null);

        bool StopBackgroundAudio();

        bool TakePicture(string controlId, out Bitmap bitmap, int ordinal = 0, string parentId = null);

        bool TakePicture(out Bitmap bitmap);

        bool Toggle(string buttonName);

        bool TryGetAllApplicationSettings(out Dictionary<string, string> values);

        bool TryGetApplicationSettings(string key, out string value);

        bool TryGetControlIsEnabled(string controlId, out bool isEnabled, int ordinal = 0, string parentId = null);

        bool TryGetPerformanceInformation(out Dictionary<string, string> values);

        bool TryGetTextFromControl(string controlId, out string text, int ordinal = 0, string parentId = null);

        bool TryGetValueFromControl(string controlId, out string textValue, int ordinal = 0, string parentId = null);

        bool VerticalScroll(string controlId, int amount);

        bool WaitForAppBarItem(string text, TimeSpan? timeout = null);

        bool WaitForControl(string controlId, int ordinal = 0, string parentId = null);

        bool WaitForControl(string controlId, TimeSpan timeout, int ordinal = 0, string parentId = null);

        bool WaitForControlOrText(string textOrControlId, int ordinal = 0, string parentId = null);

        bool WaitForControlOrText(string textOrControlId, TimeSpan timeout, int ordinal = 0, string parentId = null);

        bool WaitForControlToBeEnabled(
            string controlId, int ordinal = 0, string parentId = null, TimeSpan? timeout = null);

        bool WaitForMessageBox(string title, string message, string[] buttons);

        bool WaitForText(string text);

        bool WaitForText(string text, TimeSpan timeout);

        bool WaitForTextToChange(
            string controlId, string text, TimeSpan timeout, int ordinal = 0, string parentId = null);

        bool WaitForTextToDisappear(string text, TimeSpan timeout);

        bool WaitForControlToDisappear(string controlId, TimeSpan timeout, int ordinal = 0, string parentId = null);

        bool WaitIsAlive();

        #endregion

        string GetIsChecked(string control);
    }

    public enum PivotType
    {
        Next,

        Last
    }
}