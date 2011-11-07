// ----------------------------------------------------------------------
// <copyright file="DriverStepDefinitions.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using System;
using NUnit.Framework;
using TechTalk.SpecFlow;
using WindowsPhoneTestFramework.Server.Core.Results;

namespace WindowsPhoneTestFramework.Test.EmuSteps.StepDefinitions
{
    [Binding]
    public class DriverStepDefinitions : EmuDefinitionBase
    {
        public DriverStepDefinitions()
            : base()
        {
        }

        /*
        public DriverStepDefinitions(IConfiguration configuration)
            : base(configuration)
        {
        }
        */

        [Given(@"my app is clean installed and running$")]
        public void GivenMyAppIsCleanInstalledAndRunning()
        {
            GivenMyAppIsNotRunning();
            GivenMyAppIsUninstalled();
            GivenMyAppIsInstalled();
            GivenMyAppIsRunning();
        }

        [Given(@"my app is uninstalled$")]
        public void GivenMyAppIsUninstalled()
        {
            var result = Emu.DeviceController.ForceUninstall(Configuration.ApplicationDefinition);
            Assert.That(result == UninstallationResult.NotInstalled || result == UninstallationResult.Success);
        }

        [Given(@"my app is installed$")]
        public void GivenMyAppIsInstalled()
        {
            bool installSucceeded = false;
            try
            {
                AttemptToInstallApp();
                installSucceeded = true;
            }
            catch (System.IO.FileLoadException fileLoadException)
            {
                StepFlowOutputHelpers.WriteException("File load problem seen while installing - will try workaround of waiting 15 seconds and then installing again", fileLoadException);
            }

            if (!installSucceeded)
            {
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(15.0));
                AttemptToInstallApp();
                StepFlowOutputHelpers.Write("app installed - workaround worked");
            }
        }

        private void AttemptToInstallApp()
        {
            var result = Emu.DeviceController.Install(Configuration.ApplicationDefinition);
            Assert.That(result == InstallationResult.AlreadyInstalled || result == InstallationResult.Success);
        }

        [Given(@"my app is not running$")]
        public void GivenMyAppIsNotRunning()
        {
            var result = Emu.DeviceController.Stop(Configuration.ApplicationDefinition);
            Assert.That(result == StopResult.Success || result == StopResult.NotRunning || result == StopResult.NotInstalled);
        }

        [Given(@"my app is running$")]
        public void GivenMyAppIsRunning()
        {
            ThenIStartMyAppAndWaitForItToStart();
        }

        [Then(@"my app is running")]
        public void ThenMyAppIsAlive()
        {
            var ping = Emu.ApplicationAutomationController.LookIsAlive();
            Assert.IsTrue(ping, "App not alive - ping failed");
        }

        [Then("I start my app and wait for it to start$")]
        public void ThenIStartMyAppAndWaitForItToStart()
        {
            Then("I start my app");
            Then("my app is running");
        }

        [Then("I start my app")]
        public void ThenIStartMyApp()
        {
            var start = Emu.DeviceController.Start(Configuration.ApplicationDefinition);
            Assert.That(start == StartResult.Success, "failed to start my app - result " + start);
        }

        [Then(@"my app is not running")]
        public void ThenMyAppIsNotAlive()
        {
            var ping = Emu.ApplicationAutomationController.LookIsAlive();
            Assert.IsFalse(ping, "App was alive - ping succeeded");
        }
    }
}
