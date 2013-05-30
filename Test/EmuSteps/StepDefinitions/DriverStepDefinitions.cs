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
        private const int MaxStartupTimeInSeconds = 45;

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

        [StepDefinition(@"my app is clean installed and running$")]
        public void GivenMyAppIsCleanInstalledAndRunning()
        {
            GivenMyAppIsStopped();
            GivenMyAppIsUninstalled();
            GivenMyAppIsInstalled();
            GivenMyAppIsRunning(MaxStartupTimeInSeconds);
        }

        [StepDefinition(@"my app is uninstalled$")]
        public void GivenMyAppIsUninstalled()
        {
            AssertDeviceExceptions(() =>
                                       {
                                           var result = Emu.DeviceController.ForceUninstall(Configuration.ApplicationDefinition);
                                           Assert.That(result == UninstallationResult.NotInstalled ||
                                                       result == UninstallationResult.Success);
                                       });
        }

        [StepDefinition(@"my app is installed$")]
        public void GivenMyAppIsInstalled()
        {
            AssertDeviceExceptions(() =>
                                       {
                                           bool installSucceeded = false;
                                           try
                                           {
                                               AttemptToInstallApp();
                                               installSucceeded = true;
                                           }
                                           catch (System.IO.FileLoadException fileLoadException)
                                           {
                                               StepFlowOutputHelpers.WriteException(
                                                   "File load problem seen while installing - will try workaround of waiting 15 seconds and then installing again",
                                                   fileLoadException);
                                           }

                                           if (!installSucceeded)
                                           {
                                               System.Threading.Thread.Sleep(TimeSpan.FromSeconds(15.0));
                                               AttemptToInstallApp();
                                               StepFlowOutputHelpers.Write("app installed - workaround worked");
                                           }
                                       });
        }

        private void AttemptToInstallApp()
        {
            AssertDeviceExceptions(() =>
                                       {
                                           var result = Emu.DeviceController.Install(Configuration.ApplicationDefinition);
                                           Assert.That(result == InstallationResult.AlreadyInstalled ||
                                                       result == InstallationResult.Success);
                                       });
        }

        [StepDefinition(@"my app is stopped$")]
        public void GivenMyAppIsStopped()
        {
            AssertDeviceExceptions(() =>
                                       {
                                           var result = Emu.DeviceController.Stop(Configuration.ApplicationDefinition);
                                           Assert.That(result == StopResult.Success || result == StopResult.NotRunning ||
                                                       result == StopResult.NotInstalled);
                                           if (result == StopResult.Success)
                                           {
                                               // following experiments and research by MrGoodCat - http://www.pitorque.de/MisterGoodcat/
                                               // we've discovered that the app can stay in memory even after stop has been called (and even after it is uninstalled!)
                                               // we believe this is to allow tombstoning to occur, but it has surprised us
                                               // to cope with this, we do a check here that the application really has stopped.
                                               // Note that we choose 15 seconds as the maximum wait time - this is based on a maximum 10 seconds for tombstoning, plus a little safety margin
                                               ThenIWaitNSecondsForMyAppToBeStopped(15.0);
                                           }
                                       });
        }

        [Given(@"my app is running$")]
        public void GivenMyAppIsRunning()
        {
            ThenIStartMyAppAndWaitForItToStart();
        }

        [Given(@"my app is running within (\d+\.?\d*) seconds$")]
        [When(@"my app is running within (\d+\.?\d*) seconds$")]
        public void GivenMyAppIsRunning(double numSeconds)
        {
            ThenIStartMyAppAndWaitForItToStart(numSeconds);
        }

        [Then(@"my app is running$")]
        [When(@"my app is running$")]
        public void ThenMyAppIsAlive()
        {
            var ping = Emu.ApplicationAutomationController.LookIsAlive();
            Assert.IsTrue(ping, "App not alive - ping failed");
        }

        [StepDefinition(@"my app is stopped within (\d+\.?\d*) seconds$")]
        public void ThenIWaitNSecondsForMyAppToBeStopped(double numSeconds)
        {            
            Assert.IsTrue(WaitNSecondsForAppToBeExpectedAliveState(false, numSeconds), string.Format("App is still alive"));
        }

        [Then(@"my app is running within (\d+\.?\d*) seconds$")]
        public void ThenIWaitNSecondsForMyAppToBeAlive(double numSeconds)
        {
            Assert.IsTrue(WaitNSecondsForAppToBeExpectedAliveState(true, numSeconds), string.Format("App is not yet alive"));
        }

        [StepDefinition(@"I start my app and wait (\d+\.?\d*) seconds for it to start$")]
        public void ThenIStartMyAppAndWaitForItToStart(double numSeconds)
        {
            ThenIStartMyApp();
            ThenIWaitNSecondsForMyAppToBeAlive(numSeconds);
        }

        [StepDefinition("I start my app and wait for it to start$")]
        public void ThenIStartMyAppAndWaitForItToStart()
        {
            ThenIStartMyApp();
            ThenMyAppIsAlive();
        }

        [StepDefinition("I start my app$")]
        public void ThenIStartMyApp()
        {
            AssertDeviceExceptions(() =>
                                       {
                                           var start = Emu.DeviceController.Start(Configuration.ApplicationDefinition);
                                           Assert.That(start == StartResult.Success,
                                                       "failed to start my app - result " + start);
                                       });
        }

        [StepDefinition(@"my app is not running")]
        public void ThenMyAppIsNotAlive()
        {
            var ping = Emu.ApplicationAutomationController.LookIsAlive();
            Assert.IsFalse(ping, "App was alive - ping succeeded");
        }

        private static void AssertDeviceExceptions(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                if (ex.GetType().Name.EndsWith("SmartDeviceException"))
                {
                    NUnit.Framework.Assert.Ignore("SmartDeviceException in test setup " + ex.Message);
                }
                else
                {
                    throw;
                }
            }
        }

#warning TODO - there is already a nice WaitForTestSuccess method inside the ApplicationAutomationController - this code duplicates it :/
        private bool WaitNSecondsForAppToBeExpectedAliveState(bool isExpectedAlive, double numSeconds)
        {
            var startTimeUtc = DateTime.UtcNow;

            //var count = 0;
            while ((DateTime.UtcNow - startTimeUtc).TotalSeconds <= numSeconds)
            {
                var ping = Emu.ApplicationAutomationController.LookIsAlive();
                //count++;
                if (ping == isExpectedAlive)
                    return true; // success - app is in expected state
            }

            // could trace count here...
            return false;
        }
    }
}
