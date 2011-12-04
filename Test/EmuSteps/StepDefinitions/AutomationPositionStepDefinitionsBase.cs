// ----------------------------------------------------------------------
// <copyright file="AutomationPositionStepDefinitionsBase.cs" company="Expensify">
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
using WindowsPhoneTestFramework.Server.Core.Tangibles;

namespace WindowsPhoneTestFramework.Test.EmuSteps.StepDefinitions
{
    public class AutomationPositionStepDefinitionsBase : EmuDefinitionBase
    {
        protected bool IsControlVisible(string controlId)
        {
            var position = Emu.ApplicationAutomationController.GetPositionOfControl(controlId);
            return IsPositionVisible(position);
        }

        protected bool IsPositionVisible(RectangleF position)
        {
            if (position.IsEmpty)
                return false;

            var phoneOrientation = Emu.DisplayInputController.GuessOrientation();
            return position.IsVisible(phoneOrientation);
        }

        protected void AssertPositionIsNotVisible(RectangleF position, string textTest)
        {
            Assert.False(IsPositionVisible(position), "Position of is offscreen, text:'{0}', position:{1}", textTest, position);
        }

        protected void AssertPositionIsVisible(RectangleF position, string textTest)
        {
            Assert.True(IsPositionVisible(position), "Position of is offscreen, text:'{0}', position:{1}", textTest, position);
        }
    }
}