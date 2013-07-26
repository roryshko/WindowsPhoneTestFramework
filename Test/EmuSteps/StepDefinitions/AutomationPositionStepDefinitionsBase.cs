//  ----------------------------------------------------------------------
//  <copyright file="AutomationPositionStepDefinitionsBase.cs" company="Expensify">
//      (c) Copyright Expensify. http://www.expensify.com
//      This source is subject to the Microsoft Public License (Ms-PL)
//      Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//      All other rights reserved.
//  </copyright>
//  
//  Author - Stuart Lodge, Cirrious. http://www.cirrious.com
//  ------------------------------------------------------------------------

using System;
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
            bool result = true;
            var phoneOrientation = Emu.DisplayInputController.GuessOrientation();
            try
            {
                StepFlowOutputHelpers.Write("IsVisible checking position {0}, {1}, {2}, {3} in orientation {4}",
                                            position.Left, position.Top, position.Width, position.Height,
                                            phoneOrientation);
                position.IsVisible(phoneOrientation);
            }
            catch (Exception ex)
            {
                StepFlowOutputHelpers.Write("IsVisible exception {0}, {1}", ex.GetType().Name, ex.Message);
                result = false;
            }
            return result;
        }

        protected void AssertPositionIsNotVisible(RectangleF position, string textTest)
        {
            Assert.False(IsPositionVisible(position), "Position of is offscreen, text:'{0}', position:{1}", textTest,
                         position);
        }

        protected void AssertPositionIsVisible(RectangleF position, string textTest)
        {
            Assert.True(IsPositionVisible(position), "Position of is offscreen, text:'{0}', position:{1}", textTest,
                        position);
        }
    }
}