// ----------------------------------------------------------------------
// <copyright file="AppConfigFileBasedConfiguration.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using System;
using System.Configuration;
using WindowsPhoneTestFramework.Server.Core;

namespace WindowsPhoneTestFramework.Test.EmuSteps
{
    public class AppConfigFileBasedConfiguration : IConfiguration
    {
        private const string EmuStepsPrefix = "EmuSteps.";
        private const string EmuStepsApplicationPrefix = EmuStepsPrefix + "Application.";
        private const string EmuStepsControllerInitialisationKeyName = EmuStepsPrefix + "ControllerInitialisation";
        private const string EmuStepsAutomationControllerKeyName = EmuStepsPrefix + "AutomationController";
        private const string EmuStepsAutomationIdentificationKeyName = EmuStepsPrefix + "AutomationIdentification";

        public string AutomationControllerName{get; set; }
        public string ControllerInitialisationString { get; set; }

        public AutomationIdentification AutomationIdentification { get; set; }

        public ApplicationDefinition ApplicationDefinition { get; set; }

        public AppConfigFileBasedConfiguration()
        {
            AutomationControllerName = ConfigurationManager.AppSettings[EmuStepsAutomationControllerKeyName];
            ControllerInitialisationString = ConfigurationManager.AppSettings[EmuStepsControllerInitialisationKeyName];
            if (string.IsNullOrEmpty(AutomationControllerName))
                AutomationControllerName = "wp";
            if (string.IsNullOrEmpty(ControllerInitialisationString))
                ControllerInitialisationString = string.Empty;

            AutomationIdentification automationIdentification;
            if (Enum.TryParse(ConfigurationManager.AppSettings[EmuStepsAutomationIdentificationKeyName], true, out automationIdentification))
                AutomationIdentification = automationIdentification;
            else
                AutomationIdentification = AutomationIdentification.TryEverything;

            ApplicationDefinition = new ApplicationDefinition();
            foreach (var key in ConfigurationManager.AppSettings.AllKeys)
            {
                if (key.StartsWith(EmuStepsApplicationPrefix) && key.Length > EmuStepsApplicationPrefix.Length)
                {
                    ApplicationDefinition.Fields[key.Substring(EmuStepsApplicationPrefix.Length)] =
                        ConfigurationManager.AppSettings[key];
                }
            }
        }
    }
}