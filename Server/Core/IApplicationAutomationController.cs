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
using System.Drawing;

namespace WindowsPhoneTestFramework.Server.Core
{
    public interface IApplicationAutomationController
    {
        bool WaitIsAlive();
        bool LookIsAlive();
        bool WaitForControlOrText(string textOrControlId);
        bool WaitForControlOrText(string textOrControlId, TimeSpan timeout);
        bool WaitForControl(string controlId);
        bool WaitForControl(string controlId, TimeSpan timeout);
        bool LookForControl(string controlId);
        bool WaitForText(string text);
        bool WaitForText(string text, TimeSpan timeout);
        bool LookForText(string text);
        bool TryGetTextFromControl(string controlId, out string text);
        bool SetTextOnControl(string controlId, string text);
        bool TryGetValueFromControl(string controlId, out string textValue);
        bool SetValueOnControl(string controlId, string value);
        bool InvokeControlTapAction(string controlId);
        RectangleF GetPositionOfControl(string controlId);
        bool TryGetControlIsEnabled(string controlId, out bool isEnabled);
        RectangleF GetPositionOfControlOrText(string textOrControlId);
        RectangleF GetPositionOfText(string text);
        bool SetFocus(string controlId);
        bool TakePicture(string controlId, out Bitmap bitmap);
        bool TakePicture(out Bitmap bitmap);
    }
}