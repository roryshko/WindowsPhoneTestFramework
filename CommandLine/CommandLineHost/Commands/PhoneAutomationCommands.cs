// ----------------------------------------------------------------------
// <copyright file="AutomationUsingProgramBase.cs" company="Expensify">
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
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;

using Args;

using WindowsPhoneTestFramework.Server.Core;

namespace WindowsPhoneTestFramework.CommandLine.CommandLineHost.Commands
{
    using WindowsPhoneTestFramework.Server.Core.Types;

    public class PhoneAutomationCommands
    {
        public IApplicationAutomationController ApplicationAutomationController { get; set; }

#warning Delete click... it's there only for people who watched the "how to" video
        [CommandLineCommand("click")]
        [Description("deprecated... this method is now replaced by invokeTap - sorry!")]
        public void Click(string whatToClick)
        {
            Console.WriteLine("Click: this method is now replaced by invokeTap - sorry!");
        }

        [CommandLineCommand("invokeTap")]
        [Description("invoke the action that a tap would normally do on the identified control - for a button this is Click, for a checkbox this is Toggle, for other controls you'll need to setup the mapping yourself using AddAutomationPeerHandlerForTapAction and AddUIElementHandlerForTapAction methods - e.g. 'invoke Button1'")]
        public void InvokeTap(string whatToClick)
        {
            var args = whatToClick.Split(':');
            //controlId:ordinal:parentId
            // TODO add this logic to the other commands which take three arguments
            if (args.Length == 1)
            {
                var result = ApplicationAutomationController.InvokeControlTapAction(args[0]);
                Console.WriteLine("invokeTap:" + result.ToString());
            }
            else if (args.Length == 3)
            {
                var result = ApplicationAutomationController.InvokeControlTapAction(args[0], int.Parse(args[1]), args[2]);
                Console.WriteLine("invokeTap: {0}", result);
            }
            else
            {
                Console.WriteLine("This method needs one or three arguments: controlId | controlId:ordinal:parentId");
            }
        }

        [CommandLineCommand("scrollIntoView")]
        [Description("scroll the identified control into view - e.g. 'scrollIntoView Button1'")]
        public void scrollIntoView(string whatToScroll)
        {
            var args = whatToScroll.Split(':');
            //controlId:ordinal:parentId
            // TODO add this logic to the other commands which take three arguments
            if (args.Length == 1)
            {
                var result = ApplicationAutomationController.ScrollIntoView(args[0]);
                Console.WriteLine("scrollIntoView:" + result.ToString());
            }
            else if (args.Length == 3)
            {
                var result = ApplicationAutomationController.ScrollIntoView(args[0], int.Parse(args[1]), args[2]);
                Console.WriteLine("scrollIntoView: {0}", result);
            }
            else
            {
                Console.WriteLine("This method needs one or three arguments: controlId | controlId:ordinal:parentId");
            }
        }

        [CommandLineCommand("navigate")]
        [Description("invoke a navigation - - e.g. 'navigate Back'")]
        public void Navigate(string direction)
        {
            if (!string.IsNullOrWhiteSpace(direction))
            {
                var result = ApplicationAutomationController.Navigate(direction);
                Console.WriteLine("navigate: " + result.ToString());
            }
            else
            {
                Console.WriteLine("This method needs one argument: back | forward");
            }
        }

        [CommandLineCommand("ping")]
        [Description("sends an 'are you alive?' message to the application to test connectivity to the app - e.g. 'ping'")]
        public void ConfirmAlive(string ignored)
        {
            var result = ApplicationAutomationController.LookIsAlive();
            Console.WriteLine("Alive:" + result.ToString());
        }

        [CommandLineCommand("lookForText")]
        [Description("looks for displayed text within the app UI - e.g. 'lookForText Page 1'")]
        public void LookForText(string whatToLookFor)
        {
            var result = ApplicationAutomationController.LookForText(whatToLookFor);
            Console.WriteLine("LookForText:" + result.ToString());
        }

        [CommandLineCommand("waitForText")]
        [Description("waits for up to 1 minute for the text to be displayed within the app UI - e.g. 'waitForText Page 2'")]
        public void WhatForText(string whatToWaitFor)
        {
            var result = ApplicationAutomationController.WaitForText(whatToWaitFor);
            Console.WriteLine("WaitForText:" + result.ToString());
        }

        [CommandLineCommand("getValue")]
        [Description("gets a value from the named control in the app UI - e.g. 'getValue checkBox1'")]
        public void GetValue(string whatToGet)
        {
            string text;
            var result = ApplicationAutomationController.TryGetValueFromControl(whatToGet, out text);
            Console.WriteLine("GetValue:" + (result ? text : "FAIL"));
        }

        [CommandLineCommand("setValue")]
        [Description("sets a value on the named control in the app UI - e.g. 'setText CheckBox1=true'")]
        public void SetValue(string whatToSetAndValue)
        {
            var items = whatToSetAndValue.Split(new char[] { '=' }, 2);
            if (items.Count() != 2)
            {
                Console.WriteLine("Incorrect syntax - require setValue id=value");
                return;
            }

            var result = ApplicationAutomationController.SetValueOnControl(items[0], items[1]);
            Console.WriteLine("SetValue:" + result);
        }

        [CommandLineCommand("getIsEnabled")]
        [Description("gets whether the named control is enabled in the app UI - e.g. 'getEnabled TextBox1'")]
        public void GetIsEnabled(string whatToGet)
        {
            bool isEnabled;
            var result = ApplicationAutomationController.TryGetControlIsEnabled(whatToGet, out isEnabled);
            Console.WriteLine("GetIsEnabled:" + (result ? isEnabled.ToString() : "FAIL"));
        }

        [CommandLineCommand("getText")]
        [Description("gets text from the named control in the app UI - e.g. 'getText TextBox1'")]
        public void GetText(string whatToGet)
        {
            string text;
            var result = ApplicationAutomationController.TryGetTextFromControl(whatToGet, out text);
            Console.WriteLine("GetText:" + (result ? text : "FAIL"));
        }

        [CommandLineCommand("setText")]
        [Description("sets text on the named control in the app UI - e.g. 'setText TextBox1=Hello World'")]
        public void SetText(string whatToSetAndValue)
        {
            var items = whatToSetAndValue.Split(new char[] { '=' }, 2);
            if (items.Count() != 2)
            {
                Console.WriteLine("Incorrect syntax - require setText id=value");
                return;
            }

            var result = ApplicationAutomationController.SetTextOnControl(items[0], items[1]);
            Console.WriteLine("SetText:" + result);
        }

        [CommandLineCommand("setFocus")]
        [Description("sets the focus to the specified control - e.g. 'setFocus TextBox1'")]
        public void SetFocus(string whichControl)
        {
            var result = ApplicationAutomationController.SetFocus(whichControl);
            Console.WriteLine("setFocus: " + result);
        }

        [CommandLineCommand("getPosition")]
        [Description("gets the position of the specified control as device screen location - e.g. 'getPosition TextBox1'")]
        public void GetPosition(string whichControl)
        {
            var position = RectangleF.Empty;
            var args = whichControl.Split(':');
            //controlId:ordinal:parentId
            // TODO add this logic to the other commands which take three arguments
            if (args.Length == 1)
            {
                position = ApplicationAutomationController.GetPositionOfControl(args[0]);
            }
            else if (args.Length == 3)
            {
                position = ApplicationAutomationController.GetPositionOfControl(args[0], int.Parse(args[1]), args[2]);
            }
            else
            {
                Console.WriteLine("This method needs one or three arguments: controlId | controlId:ordinal:parentId");
            }

            if (position == RectangleF.Empty)
            {
                Console.WriteLine("getPosition: failed");
                return;
            }

            Console.WriteLine(string.Format("getPosition: {0:0.0} {1:0.0} {2:0.0} {3:0.0}", position.Left, position.Top, position.Width, position.Height));
        }

        [CommandLineCommand("getProgress")]
        [Description("gets the progress of the specified control, which must be based on RangeBase - e.g. 'getProgress ProgressBar1'")]
        public void GetProgress(string whichControl)
        {
            var progress = ApplicationAutomationController.GetProgressOfControl(whichControl);
            if (progress.Max == double.MinValue)
            {
                Console.WriteLine("getProgress: failed");
                return;
            }

            Console.WriteLine(string.Format("getProgress: min {0:0.0} max {1:0.0} current {2:0.0} ", progress.Min, progress.Max, progress.Current));
        }

        [CommandLineCommand("selectListItem")]
        [Description("Selects the item at the given index within the specified control - e.g. 'selectListItem ResultsList1 2'")]
        public void SelectListItem(string whichControl, string index)
        {
            var result = ApplicationAutomationController.SelectListItem(whichControl, int.Parse(index));
            Console.WriteLine("selectListItem: " + result);
        }

        [CommandLineCommand("scrollH")]
        [Description("scrolls the position of e.g. a list - use -10 for PageUp, -1 for Up, 0 for non, +1 for Down, +10 for PageDown - e.g. 'scrollV ListBox1 10'")]
        public void ScrollH(string whichControl, string howMuch)
        {
            var result = ApplicationAutomationController.HorizontalScroll(whichControl, int.Parse(howMuch));
            Console.WriteLine("scrollH: " + result);
        }

        [CommandLineCommand("scrollV")]
        [Description("scrolls the position of e.g. a list - use -10 for PageUp, -1 for Up, 0 for non, +1 for Down, +10 for PageDown - e.g. 'scrollV ListBox1 10'")]
        public void ScrollV(string whichControl, string howMuch)
        {
            var result = ApplicationAutomationController.VerticalScroll(whichControl, int.Parse(howMuch));
            Console.WriteLine("scrollH: " + result);
        }

        [CommandLineCommand("screenshot")]
        [Description("requests a screenshot from the running application - provide an optional control to just picture that control - e.g. 'screenshot' or 'screenshot TextBox1'")]
        public void TakeScreenshot(string optionalControlId)
        {
            Bitmap bitmap;
            if (string.IsNullOrWhiteSpace(optionalControlId))
                optionalControlId = null;
            var result = ApplicationAutomationController.TakePicture(optionalControlId, out bitmap);
            Console.WriteLine("TakePicture: " + result);
            if (result)
            {
                var fileName = Path.GetTempFileName() + ".jpg";
                try
                {
                    bitmap.Save(fileName);
                    var process = new Process
                                      {
                                          StartInfo =
                                              {
                                                  FileName = fileName
                                              }
                                      };

                    if (!process.Start())
                        return;

                    /*
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5.0));

                    if (!process.WaitForExit(60000)) // one minute
                        process.Kill();

                    File.Delete(fileName);
                    */
                }
                catch (Exception exception)
                {
                    Console.WriteLine(string.Format("Exception seen {0} - {1}", exception.GetType().FullName, exception.Message));
                }
            }
        }

        [CommandLineCommand("elementColor")]
        [Description("get the r,g,b or background element")]
        public void ElementColor(string controlId)
        {
            string result = ApplicationAutomationController.GetControlColor(controlId);
            Console.WriteLine("select: " + result);
        }

        [CommandLineCommand("tapAppBar")]
        [Description("taps the app bar menu item with the given text")]
        public void TapAppBar(string text)
        {
            var result = ApplicationAutomationController.InvokeAppBarTap(text);
            Console.WriteLine(result ? "passed" : "failed");
        }

        [CommandLineCommand("pivot")]
        [Description("Pivots a pivot control to the next / last, or named pivot")]
        public void Pivot(string text)
        {
            var args = text.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var pivotName = args[0];
            var itemName = args[1];

            switch (itemName)
            {
                case "next":
                    ApplicationAutomationController.Pivot(pivotName, PivotType.Next);
                    break;
                case "last":
                    ApplicationAutomationController.Pivot(pivotName, PivotType.Last);
                    break;
                default:
                    ApplicationAutomationController.Pivot(pivotName, itemName);
                    break;
            }
        }

        [CommandLineCommand("toggle")]
        [Description("Toggles a named toggle button")]
        public void Toggle(string text)
        {
            var args = text.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var toggleButtonName = args[0];

            var result = ApplicationAutomationController.Toggle(toggleButtonName);
            Console.WriteLine("toggle:  {0}", result ? "passed" : "failed");
        }

        [CommandLineCommand("getSettings")]
        [Description("Gets the app settings for the current applicaiton")]
        public void GetAppSettings(string text)
        {
            Dictionary<string, string> dictionary;
            var result = ApplicationAutomationController.TryGetAllApplicationSettings(out dictionary);

            Console.WriteLine("got settings: {0}", result ? "passed" : "failed");

            if (result)
            {
                foreach (var pair in dictionary)
                {
                    Console.WriteLine("{0} : {1}", pair.Key, pair.Value);
                }
            }
        }

        [CommandLineCommand("setSettings")]
        [Description("Sets all app settings for the current application, takes a list of the form Key:Value|Key2:Value2")]
        public void SetAppSettings(string text)
        {
            var pairs = text.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(
                pairString =>
                {
                    var splitPair = pairString.Split(':');
                    return new { Key = splitPair[0], Value = splitPair[1] };
                }).ToDictionary(p => p.Key, p => p.Value);

            var result = ApplicationAutomationController.SetApplicationSettings(pairs);

            Console.WriteLine("set settings: {0}", result ? "passed" : "failed");
        }

        [CommandLineCommand("setSetting")]
        [Description("Sets all app settings for the current application, takes a co delimited pair key and value")]
        public void SetAppSetting(string key, string value)
        {
            var result = ApplicationAutomationController.SetApplicationSettings(key, value);

            Console.WriteLine("set settings: {0}", result ? "passed" : "failed");

        }

        [CommandLineCommand("stopBackgroundAudio")]
        public void StopBackgroundAudio(string text)
        {
            var result = ApplicationAutomationController.StopBackgroundAudio();

            Console.WriteLine("stop audio: {0}", result ? "success" : "fail");
        }

        [CommandLineCommand("getPerformanceInfo")]
        public void GetPerformanceInfo(string text)
        {
            Dictionary<string, string> dictionary;
            var result = ApplicationAutomationController.TryGetPerformanceInformation(out dictionary);

            Console.WriteLine("get performance information {0}", result ? "success" : "failure");
        }

        [CommandLineCommand("getIsChecked")]
        public void getVisualState(string text)
        {
            var args = text.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var control = args[0];

            var result = ApplicationAutomationController.GetIsChecked(control);

            Console.WriteLine("Visual state of {0} is {1}", control, result);
        }
    }
}