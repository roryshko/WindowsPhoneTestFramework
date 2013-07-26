//  ----------------------------------------------------------------------
//  <copyright file="IDeviceController.cs" company="Expensify">
//      (c) Copyright Expensify. http://www.expensify.com
//      This source is subject to the Microsoft Public License (Ms-PL)
//      Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//      All other rights reserved.
//  </copyright>
//  
//  Author - Stuart Lodge, Cirrious. http://www.cirrious.com
//  ------------------------------------------------------------------------

using System;
using WindowsPhoneTestFramework.Server.Core.Results;
using WindowsPhoneTestFramework.Server.Utils;

namespace WindowsPhoneTestFramework.Server.Core
{
    public interface IDeviceController : IDisposable, ITrace
    {
        IDisplayInputController DisplayInputController { get; }

        bool TryConnect();
        void ReleaseDeviceConnection();

        InstallationResult ForceInstall(ApplicationDefinition applicationDefinition);
        InstallationResult Install(ApplicationDefinition applicationDefinition);

        UninstallationResult ForceUninstall(ApplicationDefinition applicationDefinition);
        UninstallationResult Uninstall(ApplicationDefinition applicationDefinition);

        StopResult Stop(ApplicationDefinition applicationDefinition);
        StartResult Start(ApplicationDefinition applicationDefinition);
        StartResult ForceStart(ApplicationDefinition applicationDefinition);

        /// <summary>
        /// Shuts down the current device
        /// </summary>
        /// <returns>True the device process was shut down</returns>
        bool ForceDeviceShutDown();

        string GetIsolatedStorage(ApplicationDefinition applicationDefinition);

        void RestoreIsolatedStorage(ApplicationDefinition applicationDefinition, string isolatedStorage);
    }
}