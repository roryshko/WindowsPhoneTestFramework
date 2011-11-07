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
    }
}