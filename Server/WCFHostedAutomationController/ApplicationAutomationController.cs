//  ----------------------------------------------------------------------
//  <copyright file="ApplicationAutomationController.cs" company="Expensify">
//      (c) Copyright Expensify. http://www.expensify.com
//      This source is subject to the Microsoft Public License (Ms-PL)
//      Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//      All other rights reserved.
//  </copyright>
//  
//  Author - Stuart Lodge, Cirrious. http://www.cirrious.com
//  ------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using WindowsPhoneTestFramework.Server.Core;
using WindowsPhoneTestFramework.Server.WCFHostedAutomationController.Commands;
using WindowsPhoneTestFramework.Server.WCFHostedAutomationController.Interfaces;
using WindowsPhoneTestFramework.Server.WCFHostedAutomationController.Results;

namespace WindowsPhoneTestFramework.Server.WCFHostedAutomationController
{
    public class ApplicationAutomationController : IApplicationAutomationController
    {
        // for "Look" requests we are expecting faster pick up of the requests - but accept that the app may still take its time responding
        // (especially if the user is debugging!)
        private static readonly TimeSpan LookSendCommandWithin = TimeSpan.FromSeconds(1.0);

        private static readonly TimeSpan LookExpectResultWithin = TimeSpan.FromSeconds(10.0);

        private readonly IPhoneAutomationServiceControl serviceControl;

        private readonly AutomationIdentification automationIdentification;

        public ApplicationAutomationController(
            IPhoneAutomationServiceControl serviceControl, AutomationIdentification automationIdentification)
        {
            this.serviceControl = serviceControl;
            this.automationIdentification = automationIdentification;
        }

        #region IApplicationAutomationController

        public bool WaitIsAlive()
        {
            return WaitForTestSuccess(LookIsAlive, Constants.DefaultConfirmAliveTimeout);
        }

        public bool WaitPanorama()
        {
            return WaitForTestSuccess(() =>
                {
                    var command = new LookForTypeCommand
                        {
                            TypeStr = "Microsoft.Phone.Controls.Panorama",
                        };
                    var result = SyncLookExecuteCommand(command) as SuccessResult;
                    return result != null;
                }, Constants.DefaultWaitForClientAppActionTimeout);
        }

        public string GetIsChecked(string control)
        {
            var command = new GetCheckedStatusCommand {AutomationIdentifier = CreateAutomationIdentifier(control)};
            var result = SyncExecuteCommand(command);

            var successResult = result as SuccessResult;

            return successResult == null ? null : successResult.ResultText;
        }

        public bool LookIsAlive()
        {
            var command = new ConfirmAliveCommand();
            var result = SyncLookExecuteCommand(command);
            return result is SuccessResult;
        }

        public bool WaitForTextToDisappear(string text, TimeSpan timeout)
        {
            return WaitForTestSuccess(() => !LookForText(text), timeout);
        }

        public bool WaitForControlToDisappear(string controlId, TimeSpan timeout, int ordinal, string parentId)
        {
            return WaitForTestSuccess(() => !this.LookForControl(controlId, ordinal, parentId), timeout);
        }

        public bool LookForText(string text)
        {
            var command = new LookForTextCommand {Text = text};
            var result = SyncLookExecuteCommand(command);
            return result is SuccessResult;
        }

        public bool WaitForText(string text)
        {
            return WaitForText(text, Constants.DefaultWaitForClientAppActionTimeout);
        }

        public bool WaitForTextToChange(string controlId, string text, TimeSpan timeout, int ordinal, string parentId)
        {
            return WaitForTestSuccess(
                () =>
                    {
                        string txt;
                        var success = TryGetValueFromControl(controlId, out txt, ordinal, parentId);
                        return success && txt != text;
                    },
                timeout);
        }

        public bool WaitForText(string text, TimeSpan timeout)
        {
            return WaitForTestSuccess(() => LookForText(text), timeout);
        }

        public bool WaitForControlOrText(string textOrControlId, int ordinal, string parentId)
        {
            return WaitForControlOrText(textOrControlId, Constants.DefaultWaitForClientAppActionTimeout, ordinal,
                                        parentId);
        }

        public bool WaitForControlOrText(string textOrControlId, TimeSpan timeout, int ordinal, string parentId)
        {
            return WaitForTestSuccess(() => LookForControlOrText(textOrControlId, ordinal, parentId), timeout);
        }

        public bool LookForControlOrText(string textOrControlId, int ordinal, string parentId)
        {
            var controlIdentifier = CreateControlOrTextAutomationIdentifier(textOrControlId);
            var parentIdentifier = CreateControlOrTextAutomationIdentifier(parentId);

            return LookForAutomationIdentifer(controlIdentifier, ordinal, parentIdentifier);
        }

        public bool WaitForControl(string controlId, int ordinal, string parentId)
        {
            return WaitForControl(controlId, Constants.DefaultWaitForClientAppActionTimeout, ordinal, parentId);
        }

        public bool WaitForControl(string controlId, TimeSpan timeout, int ordinal, string parentId)
        {
            return WaitForTestSuccess(() => LookForControl(controlId, ordinal, parentId), timeout);
        }

        public bool LookForControl(string controlId, int ordinal, string parentId)
        {
            var controlIdentifier = CreateAutomationIdentifier(controlId);
            var parentIdentifier = CreateAutomationIdentifier(parentId);

            return LookForAutomationIdentifer(controlIdentifier, ordinal, parentIdentifier);
        }

        public bool TryGetTextFromControl(string controlId, out string text, int ordinal, string parentId)
        {
            return TryGetTextFromAutomationIdentifier(CreateAutomationIdentifier(controlId), out text, ordinal,
                                                      CreateAutomationIdentifier(parentId));
        }

        public bool SetTextOnControl(string controlId, string text, int ordinal, string parentId)
        {
            return SetTextOnAutomationIdentification(CreateAutomationIdentifier(controlId), text, ordinal,
                                                     CreateAutomationIdentifier(parentId));
        }

        public bool TryGetValueFromControl(string controlId, out string text, int ordinal, string parentId)
        {
            return TryGetValueFromAutomationIdentifier(CreateAutomationIdentifier(controlId), out text, ordinal,
                                                       CreateAutomationIdentifier(parentId));
        }

        public bool SetValueOnControl(string controlId, string value, int ordinal, string parentId)
        {
            return SetValueOnAutomationIdentification(CreateAutomationIdentifier(controlId), value, ordinal,
                                                      CreateAutomationIdentifier(parentId));
        }

        public bool InvokeControlTapAction(string controlId, int ordinal, string parentId)
        {
            var controlIdentifier = CreateAutomationIdentifier(controlId);
            var parentIdentifier = CreateAutomationIdentifier(parentId);

            var command = new InvokeControlTapActionCommand
                {
                    AutomationIdentifier = controlIdentifier,
                    Ordinal = ordinal,
                    ParentIdentifier = parentIdentifier
                };
            var result = SyncExecuteCommand(command);
            return result is SuccessResult;
        }

        public bool ScrollIntoView(string controlId, int ordinal = 0, string parentId = null)
        {
            var controlIdentifier = CreateAutomationIdentifier(controlId);
            var parentIdentifier = CreateAutomationIdentifier(parentId);

            var command = new ScrollIntoViewCommand
                {
                    AutomationIdentifier = controlIdentifier,
                    Ordinal = ordinal,
                    ParentIdentifier = parentIdentifier
                };
            var result = SyncExecuteCommand(command);
            return result is SuccessResult;
        }

        public bool WaitForMessageBox(string title, string message, string[] buttons)
        {
            return WaitForTestSuccess(() => FindMessageBox(title, message, buttons),
                                      Constants.DefaultWaitForClientAppActionTimeout);
        }

        public bool InvokeMessageboxTapAction(string buttonText)
        {
            var automationIdentifier = CreateAutomationIdentifier(buttonText);
            var command = new InvokeControlTapActionCommand
                {
                    AutomationIdentifier = automationIdentifier,
                    ParentIdentifier = new AutomationIdentifier("MessageBox", AutomationIdentification.TryElementName)
                };

            var result = SyncExecuteCommand(command);
            return result is SuccessResult;
        }

        public bool FindMessageBox(string title, string message, string[] buttons)
        {
            var automationIdentifier = CreateAutomationIdentifier(title);
            var command = new GetPositionCommand
                {
                    AutomationIdentifier = automationIdentifier,
                    ParentIdentifier = new AutomationIdentifier("MessageBox", AutomationIdentification.TryElementName)
                };
            var positionResult = SyncExecuteCommand(command) as PositionResult;
            if (positionResult == null)
            {
                return false;
            }

            return positionResult.Height + positionResult.Width > 0;
        }

        public bool TakePicture(string controlId, out Bitmap bitmap, int ordinal, string parentId)
        {
            var command = new TakePictureCommand
                {
                    AutomationIdentifier = CreateAutomationIdentifier(controlId),
                    Ordinal = ordinal,
                    ParentIdentifier = CreateAutomationIdentifier(parentId)
                };

            var result = SyncExecuteCommand(command);
            var pictureResult = result as PictureResult;
            if (pictureResult == null)
            {
                // TODO - should log the result here really
                bitmap = null;
                return false;
            }

            var bytes = Convert.FromBase64String(pictureResult.EncodedPictureBytes);
            var memoryStream = new MemoryStream(bytes);
            bitmap = new Bitmap(memoryStream);

            return true;
        }

        public bool TakePicture(out Bitmap bitmap)
        {
            return TakePicture(null, out bitmap, 0, null);
        }

        public bool HorizontalScroll(string controlId, int amount)
        {
            return CommonScroll(controlId, amount, 0);
        }

        public bool VerticalScroll(string controlId, int amount)
        {
            return CommonScroll(controlId, 0, amount);
        }

        private bool CommonScroll(string controlId, int horizontalAmount, int verticalAmount)
        {
            var command = new ScrollCommand
                {
                    AutomationIdentifier = CreateAutomationIdentifier(controlId),
                    HorizontalScroll = horizontalAmount,
                    VerticalScroll = verticalAmount
                };
            var result = SyncExecuteCommand(command);
            return result is SuccessResult;
        }

        public bool ScrollIntoViewListItem(string controlWithinItemId)
        {
            var command = new ListBoxItemScrollIntoViewCommand
                {AutomationIdentifier = CreateAutomationIdentifier(controlWithinItemId),};
            var result = SyncExecuteCommand(command);
            return result is SuccessResult;
        }

        public bool SelectListItem(string selectorName, int indexOfItemToSelect)
        {
            var command = new SelectorItemCommand
                {
                    AutomationIdentifier = CreateAutomationIdentifier(selectorName),
                    IndexOfItemToSelect = indexOfItemToSelect
                };
            var result = SyncExecuteCommand(command);
            return result is SuccessResult;
        }

        public bool SelectListItem(string controlWithinItemId)
        {
            var command = new ListBoxItemSelectCommand
                {
                    AutomationIdentifier = CreateAutomationIdentifier(controlWithinItemId),
                };
            var result = SyncExecuteCommand(command);
            return result is SuccessResult;
        }

        public RectangleF GetPositionOfText(string text)
        {
            return GetPositionOfAutomationIdentifier(CreateTextOnlyAutomationIdentifier(text), 0, null);
        }

        public RectangleF GetPositionOfControl(string controlId, int ordinal, string parentId)
        {
            return GetPositionOfAutomationIdentifier(CreateAutomationIdentifier(controlId), ordinal,
                                                     CreateAutomationIdentifier(parentId));
        }

        public RectangleF GetPositionOfControlOrText(string textOrControlId, int ordinal, string parentId)
        {
            return GetPositionOfAutomationIdentifier(CreateControlOrTextAutomationIdentifier(textOrControlId), ordinal,
                                                     CreateControlOrTextAutomationIdentifier(parentId));
        }

        private RectangleF GetPositionOfAutomationIdentifier(AutomationIdentifier controlIdentifier, int ordinal,
                                                             AutomationIdentifier parentIdentifier)
        {
            var command = new GetPositionCommand
                {
                    AutomationIdentifier = controlIdentifier,
                    Ordinal = ordinal,
                    ParentIdentifier = parentIdentifier
                };

            var result = SyncExecuteCommand(command);
            var positionResult = result as PositionResult;
            if (positionResult == null)
            {
                // TODO - should log the result here really
                return RectangleF.Empty;
            }

            return new RectangleF(
                (float) positionResult.Left,
                (float) positionResult.Top,
                (float) positionResult.Width,
                (float) positionResult.Height);
        }

        public bool SetFocus(string controlId, int ordinal, string parentId)
        {
            var command = new SetFocusCommand
                {
                    AutomationIdentifier = CreateAutomationIdentifier(controlId),
                    Ordinal = ordinal,
                    ParentIdentifier = CreateAutomationIdentifier(parentId)
                };

            var result = SyncExecuteCommand(command);
            return result is SuccessResult;
        }

        public ProgressValues GetProgressOfText(string text)
        {
            return GetProgressOfAutomationIdentifier(CreateTextOnlyAutomationIdentifier(text));
        }

        public ProgressValues GetProgressOfControl(string controlId)
        {
            return GetProgressOfAutomationIdentifier(CreateAutomationIdentifier(controlId));
        }

        public ProgressValues GetProgressOfControlOrText(string textOrControlId)
        {
            return GetProgressOfAutomationIdentifier(CreateControlOrTextAutomationIdentifier(textOrControlId));
        }

        private ProgressValues GetProgressOfAutomationIdentifier(AutomationIdentifier automationIdentifier)
        {
            var command = new GetProgressCommand {AutomationIdentifier = automationIdentifier};
            var result = SyncExecuteCommand(command);
            var progressResult = result as ProgressResult;
            if (progressResult == null)
            {
                // TODO - should log the result here really
                return new ProgressValues();
            }

            return new ProgressValues(
                progressResult.Min,
                progressResult.Max,
                progressResult.Current);
        }

        public bool WaitForControlToBeEnabled(string controlId, int ordinal, string parentId, TimeSpan? timeout = null)
        {
            timeout = timeout.HasValue ? timeout : Constants.DefaultWaitForClientAppActionTimeout;

            return WaitForTestSuccess(
                () =>
                    {
                        bool isEnabled;
                        TryGetControlIsEnabled(controlId, out isEnabled, ordinal, parentId);
                        return isEnabled;
                    }, timeout.Value);
        }

        public bool TryGetControlIsEnabled(string controlId, out bool isEnabled, int ordinal, string parentId)
        {
            isEnabled = false;
            var command = new GetIsEnabledCommand
                {
                    AutomationIdentifier = CreateAutomationIdentifier(controlId),
                    Ordinal = ordinal,
                    ParentIdentifier = CreateAutomationIdentifier(parentId)
                };

            var result = SyncExecuteCommand(command);
            var successResult = result as SuccessResult;
            return successResult != null && bool.TryParse(successResult.ResultText, out isEnabled);
        }

        public string GetControlColor(string controlIdOrText, int ordinal, string parentId)
        {
            var command = new GetColorCommand
                {
                    AutomationIdentifier = CreateAutomationIdentifier(controlIdOrText),
                    Ordinal = ordinal,
                    ParentIdentifier = CreateAutomationIdentifier(parentId)
                };

            var result = SyncExecuteCommand(command);
            var successResult = result as SuccessResultColor;
            return successResult == null ? string.Empty : successResult.ResultColor;
        }

        public bool ControlContainsImage(string controlId, string imageName, int ordinal, string parentId)
        {
            var command = new ControlContainsImageCommand
                {
                    AutomationIdentifier = CreateAutomationIdentifier(controlId),
                    ImageName = imageName,
                    Ordinal = ordinal,
                    ParentIdentifier = CreateAutomationIdentifier(parentId)
                };

            var result = SyncExecuteCommand(command);
            return result is SuccessResult;
        }

        public bool TryGetApplicationSettings(string key, out string value)
        {
            var command = new GetApplicationSettingCommand {Key = key};
            var result = SyncExecuteCommand(command);
            var successResult = result as SuccessResult;
            if (successResult == null)
            {
                value = null;
                return false;
            }

            value = successResult.ResultText;
            return true;
        }

        public bool TryGetAllApplicationSettings(out Dictionary<string, string> values)
        {
            var command = new GetApplicationSettingsCommand();

            var result = SyncExecuteCommand(command) as DictionaryResult;

            // Is this the right thing? Or should it be an empty dictionary
            values = null;

            if (result != null)
            {
                values = result.Results;

                Console.WriteLine("Retrieved app settings:");

                foreach (var pair in result.Results)
                {
                    Console.WriteLine("{0} : {1}", pair.Key, pair.Value);
                }
            }

            return result != null;
        }

        public bool TryGetPerformanceInformation(out Dictionary<string, string> values)
        {
            var command = new GetPerformanceCommand();

            var result = SyncExecuteCommand(command) as DictionaryResult;

            // Is this the right thing? Or should it be an empty dictionary
            values = null;

            if (result != null)
            {
                values = result.Results;

                Console.WriteLine("Retrieved app performance information:");

                foreach (var pair in result.Results)
                {
                    long v;
                    var islong = long.TryParse(pair.Value, out v);
                    if (islong)
                    {
                        Console.WriteLine("{0} : {1}", pair.Key, string.Format("{0}MB", v/1000000));
                    }
                    else
                    {
                        Console.WriteLine("{0} : {1}", pair.Key, pair.Value);
                    }
                }
            }

            return result != null;
        }

        public bool SetApplicationSettings(string key, string value)
        {
            var command = new SetApplicationSettingCommand {Key = key, Value = value};
            var result = SyncExecuteCommand(command);
            return result is SuccessResult;
        }

        public bool SetApplicationSettings(Dictionary<string, string> values)
        {
            if (values == null)
            {
                return false;
            }

            Console.WriteLine("Setting app settings.");

            foreach (var pair in values)
            {
                Console.WriteLine("{0} : {1}", pair.Key, pair.Value);
            }

            var command = new SetApplicationSettingsCommand {Settings = values};

            var result = this.SyncExecuteCommand(command);

            return result is SuccessResult;
        }

        public bool StopBackgroundAudio()
        {
            var command = new BackgroundAudioCommand {Command = AudioInstruction.Stop};

            return SyncExecuteCommand(command) is SuccessResult;
        }

        public bool InvokeAppBarTap(string text)
        {
            var command = new InvokeAppBarTapCommand {AutomationIdentifier = CreateAutomationIdentifier(text)};

            return SyncExecuteCommand(command) is SuccessResult;
        }

        public bool LookForAppBarItem(string text)
        {
            var command = new LookForAppBarItemCommand {AutomationIdentifier = CreateAutomationIdentifier(text)};
            return SyncExecuteCommand(command) is SuccessResult;
        }

        public bool WaitForAppBarItem(string text, TimeSpan? timeout)
        {
            return WaitForTestSuccess(() => LookForAppBarItem(text),
                                      timeout ?? Constants.DefaultWaitForClientAppActionTimeout);
        }

        public bool Navigate(string direction)
        {
            var command = new NavigateCommand {Direction = direction};

            return SyncExecuteCommand(command) is SuccessResult;
        }

        public bool Pivot(string pivotName, PivotType pivot)
        {
            var command = new PivotCommand
                {
                    AutomationIdentifier = CreateAutomationIdentifier(pivotName),
                    PivotLast = pivot == PivotType.Last,
                    PivotNext = pivot == PivotType.Next
                };

            var result = SyncExecuteCommand(command);
            Thread.Sleep(TimeSpan.FromSeconds(3)); // Wait for pivot animations to complete

            return result is SuccessResult;
        }

        public bool Pivot(string pivotName, string itemName)
        {
            var command = new PivotCommand
                {
                    AutomationIdentifier = CreateAutomationIdentifier(pivotName),
                    PivotName = itemName
                };

            var result = SyncExecuteCommand(command);
            Thread.Sleep(TimeSpan.FromSeconds(3)); // Wait for pivot animations to complete

            return result is SuccessResult;
        }

        public bool Toggle(string buttonName)
        {
            var command = new ToggleButtonCommand
                {
                    AutomationIdentifier = CreateAutomationIdentifier(buttonName)
                };

            return SyncExecuteCommand(command) is SuccessResult;
        }

        #endregion // IApplicationAutomationController

        #region Private methods

        private static AutomationIdentifier CreateTextOnlyAutomationIdentifier(string text)
        {
            return new AutomationIdentifier {DisplayedText = text};
        }

        private AutomationIdentifier CreateControlOrTextAutomationIdentifier(string textOrControlId)
        {
            var toReturn = CreateAutomationIdentifier(textOrControlId);
            if (toReturn != null) toReturn.DisplayedText = textOrControlId;
            return toReturn;
        }

        private AutomationIdentifier CreateAutomationIdentifier(string id)
        {
            return (! string.IsNullOrEmpty(id)) ? new AutomationIdentifier(id, automationIdentification) : null;
        }

        private bool LookForAutomationIdentifer(AutomationIdentifier controlIdentifier, int ordinal,
                                                AutomationIdentifier parentIdentifier)
        {
            var command = new GetPositionCommand
                {
                    AutomationIdentifier = controlIdentifier,
                    Ordinal = ordinal,
                    ParentIdentifier = parentIdentifier,
                    ReturnEmptyIfNotVisible = true
                };

            var result = SyncLookExecuteCommand(command) as PositionResult;
            if (result == null)
            {
                return false;
            }

            // check that position is not empty
            return result.Width + result.Height > 0.0;
        }

        private ResultBase SyncLookExecuteCommand(CommandBase command)
        {
            ResultBase toReturn = null;
            var manualResetEvent = new ManualResetEvent(false);
            serviceControl.AddCommand(
                command,
                result =>
                    {
                        toReturn = result;
                        LogFailedMessage(toReturn);
                        manualResetEvent.Set();
                    },
                LookSendCommandWithin,
                LookExpectResultWithin);
            manualResetEvent.WaitOne();
            return toReturn;
        }

        private ResultBase SyncExecuteCommand(CommandBase command)
        {
            ResultBase toReturn = null;
            var manualResetEvent = new ManualResetEvent(false);
            serviceControl.AddCommand(
                command,
                result =>
                    {
                        toReturn = result;
                        LogFailedMessage(toReturn);
                        manualResetEvent.Set();
                    });
            manualResetEvent.WaitOne();
            return toReturn;
        }

        private static void LogFailedMessage(ResultBase toReturn)
        {
            var failedResultBase = toReturn as FailedResultBase;
            if (failedResultBase != null)
            {
                Console.WriteLine("WCF command messages ->: {0}", failedResultBase.FailureText);
            }
        }

        private static bool WaitForTestSuccess(Func<bool> test, TimeSpan timeout)
        {
            var start = DateTime.UtcNow;

            do
            {
                if (test())
                {
                    return true;
                }
            } while (DateTime.UtcNow - start < timeout);

            return false;
        }

        private bool SetTextOnAutomationIdentification(AutomationIdentifier controlIdentifier, string text, int ordinal,
                                                       AutomationIdentifier parentIdentifier)
        {
            var command = new SetTextCommand
                {
                    AutomationIdentifier = controlIdentifier,
                    Text = text,
                    Ordinal = ordinal,
                    ParentIdentifier = parentIdentifier
                };

            var result = SyncExecuteCommand(command);
            var successResult = result as SuccessResult;
            return successResult != null;
        }

        private bool SetValueOnAutomationIdentification(AutomationIdentifier controlIdentifier, string textValue,
                                                        int ordinal, AutomationIdentifier parentIdentifier)
        {
            var command = new SetValueCommand
                {
                    AutomationIdentifier = controlIdentifier,
                    TextValue = textValue,
                    Ordinal = ordinal,
                    ParentIdentifier = parentIdentifier
                };

            var result = SyncExecuteCommand(command);
            var successResult = result as SuccessResult;
            return successResult != null;
        }

        private bool TryGetTextFromAutomationIdentifier(AutomationIdentifier controlIdentifier, out string text,
                                                        int ordinal, AutomationIdentifier parentIdentifier)
        {
            text = null;
            var command = new GetTextCommand
                {
                    AutomationIdentifier = controlIdentifier,
                    Ordinal = ordinal,
                    ParentIdentifier = parentIdentifier
                };

            var result = SyncExecuteCommand(command);
            var successResult = result as SuccessResult;
            if (successResult == null)
            {
                return false;
            }

            text = successResult.ResultText;
            return true;
        }

        private bool TryGetValueFromAutomationIdentifier(AutomationIdentifier controlIdentifier, out string value,
                                                         int ordinal, AutomationIdentifier parentIdentifier)
        {
            value = null;
            var command = new GetValueCommand
                {
                    AutomationIdentifier = controlIdentifier,
                    Ordinal = ordinal,
                    ParentIdentifier = parentIdentifier
                };

            var result = SyncExecuteCommand(command);
            var successResult = result as SuccessResult;
            if (successResult == null)
            {
                return false;
            }

            value = successResult.ResultText;
            return true;
        }

        #endregion
    }
}