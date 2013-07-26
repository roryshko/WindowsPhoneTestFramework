//  ----------------------------------------------------------------------
//  <copyright file="FluentAutomation.cs" company="Expensify">
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
using WindowsPhoneTestFramework.Server.Core;

namespace WindowsPhoneTestFramework.Server.FluentCore
{
#warning Still need to add Device and UI features to this code!
#warning Also missing latest newest commands - e.g. scroll and select list items

    public abstract class FluentBase
    {
        protected readonly IAutomationController Automation;

        protected FluentBase(IAutomationController automation)
        {
            Automation = automation;
        }
    }

    public class Automation : FluentBase
    {
        public Automation(IAutomationController automation) : base(automation)
        {
        }

        public Control Control(string which)
        {
            return new Control(Automation, which);
        }

        public Text Text(string which)
        {
            return new Text(Automation, which);
        }

        public ControlOrText ControlOrText(string which)
        {
            return new ControlOrText(Automation, which);
        }

        public void WaitFor()
        {
            if (!Automation.ApplicationAutomationController.WaitIsAlive())
                throw new AutomationException("Wait failed");
        }

        public bool IsRunning
        {
            get { return Automation.ApplicationAutomationController.LookIsAlive(); }
        }

        public Bitmap TakePicture()
        {
            Bitmap bitmap;
            if (!Automation.ApplicationAutomationController.TakePicture(out bitmap))
                throw new AutomationException("TakePicture failed");
            return bitmap;
        }
    }

    public class Text : FluentBase
    {
        private readonly string _text;

        public Text(IAutomationController automation, string text)
            : base(automation)
        {
            _text = text;
        }

        public RectangleF Position
        {
            get
            {
                // TODO - should this throw an exception on an empty rectangle?
                return Automation.ApplicationAutomationController.GetPositionOfText(_text);
            }
        }

        public void WaitFor()
        {
            if (!Automation.ApplicationAutomationController.WaitForText(_text))
                throw new AutomationException("Wait failed for " + _text);
        }

        public void WaitFor(TimeSpan timeOut)
        {
            if (!Automation.ApplicationAutomationController.WaitForText(_text, timeOut))
                throw new AutomationException("Wait failed for " + _text);
        }
    }

    public class ControlOrText : FluentBase
    {
        private readonly string _controlOrTextId;

        public ControlOrText(IAutomationController automation, string controlOrTextId)
            : base(automation)
        {
            _controlOrTextId = controlOrTextId;
        }

        public RectangleF Position
        {
            get
            {
                // TODO - should this throw an exception on an empty rectangle?
                return Automation.ApplicationAutomationController.GetPositionOfControlOrText(_controlOrTextId);
            }
        }

        public void WaitFor()
        {
            if (!Automation.ApplicationAutomationController.WaitForControlOrText(_controlOrTextId))
                throw new AutomationException("Wait failed for " + _controlOrTextId);
        }

        public void WaitFor(TimeSpan timeOut)
        {
            if (!Automation.ApplicationAutomationController.WaitForControlOrText(_controlOrTextId, timeOut))
                throw new AutomationException("Wait failed for " + _controlOrTextId);
        }
    }

    public class Control : FluentBase
    {
        private readonly string _controlId;

        public Control(IAutomationController automation, string controlId)
            : base(automation)
        {
            _controlId = controlId;
        }

        public bool IsEnabled
        {
            get
            {
                bool isEnabled;
                if (!Automation.ApplicationAutomationController.TryGetControlIsEnabled(_controlId, out isEnabled))
                    throw new AutomationException("Unable to get enabled state for " + _controlId);

                return isEnabled;
            }
        }

        public string Text
        {
            get
            {
                string text;
                if (!Automation.ApplicationAutomationController.TryGetTextFromControl(_controlId, out text))
                    throw new AutomationException("Unable to get text for " + _controlId);

                return text;
            }
            set
            {
                if (!Automation.ApplicationAutomationController.SetTextOnControl(_controlId, value))
                    throw new AutomationException("Unable to set text for " + _controlId);
            }
        }

        public string Value
        {
            get
            {
                string text;
                if (!Automation.ApplicationAutomationController.TryGetValueFromControl(_controlId, out text))
                    throw new AutomationException("Unable to get value for " + _controlId);

                return text;
            }
            set
            {
                if (!Automation.ApplicationAutomationController.SetValueOnControl(_controlId, value))
                    throw new AutomationException("Unable to set value for " + _controlId);
            }
        }

        public RectangleF Position
        {
            get
            {
                // TODO - should this throw an exception on an empty rectangle?
                return Automation.ApplicationAutomationController.GetPositionOfControl(_controlId);
            }
        }

        public void Tap()
        {
            if (!Automation.ApplicationAutomationController.InvokeControlTapAction(_controlId))
                throw new AutomationException("Unable to tap " + _controlId);
        }

        public void WaitFor()
        {
            if (!Automation.ApplicationAutomationController.WaitForControl(_controlId))
                throw new AutomationException("Wait failed for " + _controlId);
        }

        public void WaitFor(TimeSpan timeOut)
        {
            if (!Automation.ApplicationAutomationController.WaitForControl(_controlId, timeOut))
                throw new AutomationException("Wait failed for " + _controlId);
        }

        public bool IsPresent
        {
            get { return Automation.ApplicationAutomationController.LookForControl(_controlId); }
        }

        public void SetFocus()
        {
            if (!Automation.ApplicationAutomationController.SetFocus(_controlId))
                throw new AutomationException("SetFocus failed for " + _controlId);
        }

        public Bitmap TakePicture()
        {
            Bitmap bitmap;
            if (!Automation.ApplicationAutomationController.TakePicture(_controlId, out bitmap))
                throw new AutomationException("TakePicture failed for " + _controlId);
            return bitmap;
        }
    }
}