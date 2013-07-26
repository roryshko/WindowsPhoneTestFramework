// ----------------------------------------------------------------------
// <copyright file="EmuDefinitionBase.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using System;
using System.Linq;
using NUnit.Framework;
using TechTalk.SpecFlow;
using WindowsPhoneTestFramework.Server.Core;
using WindowsPhoneTestFramework.Test.EmuSteps.ExtensionMethods;
using System.Diagnostics;

namespace WindowsPhoneTestFramework.Test.EmuSteps
{
    public class EmuDefinitionBase : ConfigurableDefinitionBase
    {
        public EmuDefinitionBase()
        {
        }

        public EmuDefinitionBase(IConfiguration configuration)
            : base(configuration)
        {
        }

        protected IAutomationController Emu
        {
            get { return StepFlowContextHelpers.GetEmuAutomationController(ScenarioContext.Current, Configuration); }
        }

        protected void DisposeOfEmu()
        {
            StepFlowContextHelpers.DisposeOfEmu(ScenarioContext.Current);
        }

        protected void IterateOverNameTable(Table table, Action<string> action)
        {
            Assert.IsTrue(table.Header.Contains("name"));
            var names = table.Rows.Select(x => x["name"]);
            foreach (var name in names)
            {
                action(name);
            }
        }

        protected void IterateOverNameTable(Table table, string actionMessage)
        {
            IterateOverNameTable(table, (name) => Then(string.Format(actionMessage, name)));
        }

        protected void IterateOverNameValueTable(Table table, string actionMessage)
        {
            IterateOverNameValueTable(table, (name, value) => Then(string.Format(actionMessage, name, value)));
        }

        protected void IterateOverNameValueTable(Table table, Action<string, string> action)
        {
            Assert.IsTrue(table.Header.Contains("name"));
            Assert.IsTrue(table.Header.Contains("value"));
            var pairs = table.Rows.Select(x => new { name = x["name"], value = x["value"].ReplaceUniqueTokenIfNecessary() });

            foreach (var pair in pairs)
            {
                action(pair.name, pair.value);
            }
        }

        /// <summary>
        /// Converts the gherkin name to a CamelCase Automation client friendly name.
        /// </summary>
        /// <param name="named">"continue"</param>
        /// <param name="type">"button"</param>
        /// <returns>"ContinueButton"</returns>
        protected static string ControlName(string named, string type)
        {
            var controlName = string.Empty;
            var name = named.Trim();

            if (!string.IsNullOrWhiteSpace(type) && !named.EndsWith(type))
            {
                name = (name + " " + type.Trim()).Trim();
            }

            foreach (var part in name.Split(' '))
            {
                controlName += part[0].ToString().ToUpper() + part.Substring(1);
            }

            Debug.WriteLine("Looking for control " + controlName);
            return controlName;
        }

#warning Investigate this commented out GetIndexOfOrdinal - can it be deleted?
        //protected static int GetIndexOfOrdinal(string ordinal)
        //{
        //    int index = 0;
        //    if (!string.IsNullOrWhiteSpace(ordinal))
        //    {
        //        // removes rd, st, nd, th or any other two letters

        //        ordinal = ordinal.Trim();

        //        Debug.WriteLine(ordinal.Substring(0, ordinal.Length - 2));
        //        if (int.TryParse(ordinal.Substring(0, ordinal.Length - 2), out index))
        //            index--;
        //    }
        //    return index;
        //}

#warning Investigate this commented out ControlName - can it be deleted?
        //protected static string ControlName(string named, string type, string ordinal)
        //{
        //    var index = -1;
        //    if (!string.IsNullOrWhiteSpace(ordinal))
        //    {
        //        Debug.WriteLine(ordinal.Substring(0, ordinal.Length - 3));
        //        if (int.TryParse(ordinal.Substring(0, ordinal.Length - 3), out index))
        //            index--;
        //    }

        //    return ControlName(named, type, index);
        //}

#warning Investigate this commented out ControlName - can it be deleted?
        //protected static string ControlName(string named, string type, int index)
        //{

        //}

        protected void WaitForParent(string parentname)
        {
            if (!string.IsNullOrEmpty(parentname))
            {
                Assert.IsTrue(
                    Emu.ApplicationAutomationController.WaitForControl(parentname),
                    "Failed while waiting for parent of name '{0}'",
                    parentname);
                Assert.IsTrue(
                    Emu.ApplicationAutomationController.WaitForControlToBeEnabled(parentname),
                    "Failed while waiting for parent of name '{0}' to be enabled",
                    parentname);
            }
        }
    }
}