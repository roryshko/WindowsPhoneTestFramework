//  ----------------------------------------------------------------------
//  <copyright file="ConfigurableDefinitionBase.cs" company="Expensify">
//      (c) Copyright Expensify. http://www.expensify.com
//      This source is subject to the Microsoft Public License (Ms-PL)
//      Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//      All other rights reserved.
//  </copyright>
//  
//  Author - Stuart Lodge, Cirrious. http://www.cirrious.com
//  ------------------------------------------------------------------------

using TechTalk.SpecFlow;

namespace WindowsPhoneTestFramework.Test.EmuSteps
{
    public class ConfigurableDefinitionBase : Steps
    {
        private readonly IConfiguration _configuration;

        protected IConfiguration Configuration
        {
            get { return _configuration; }
        }

        public ConfigurableDefinitionBase()
            : this(new AppConfigFileBasedConfiguration())
        {
        }

        public ConfigurableDefinitionBase(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    }
}