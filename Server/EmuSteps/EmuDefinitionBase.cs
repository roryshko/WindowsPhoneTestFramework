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

using TechTalk.SpecFlow;
using WindowsPhoneTestFramework.Server.Core;

namespace WindowsPhoneTestFramework.Server.EmuSteps
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
    }
}