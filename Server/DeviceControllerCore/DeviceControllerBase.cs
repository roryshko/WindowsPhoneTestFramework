//  ----------------------------------------------------------------------
//  <copyright file="DeviceControllerBase.cs" company="Expensify">
//      (c) Copyright Expensify. http://www.expensify.com
//      This source is subject to the Microsoft Public License (Ms-PL)
//      Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//      All other rights reserved.
//  </copyright>
//  
//  Author - Stuart Lodge, Cirrious. http://www.cirrious.com
//  ------------------------------------------------------------------------

using System;
using WindowsPhoneTestFramework.Server.Core;
using WindowsPhoneTestFramework.Server.Core.Results;
using WindowsPhoneTestFramework.Server.Utils;

namespace WindowsPhoneTestFramework.Server.DeviceController
{
    public abstract class DeviceControllerBase : TraceBase, IDeviceController
    {
        public IDisplayInputController DisplayInputController { get; protected set; }

        ~DeviceControllerBase()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            // nothing to do...
        }

        public abstract bool TryConnect();
        public abstract void ReleaseDeviceConnection();
        public abstract InstallationResult Install(ApplicationDefinition applicationDefinition);
        public abstract UninstallationResult Uninstall(ApplicationDefinition applicationDefinition);
        public abstract StopResult Stop(ApplicationDefinition applicationDefinition);
        public abstract StartResult Start(ApplicationDefinition applicationDefinition);

        public abstract string GetIsolatedStorage(ApplicationDefinition applicationDefinition);

        public abstract void RestoreIsolatedStorage(ApplicationDefinition applicationDefinition, string isolatedStorage);

        public InstallationResult ForceInstall(ApplicationDefinition applicationDefinition)
        {
            ForceUninstall(applicationDefinition);
            return Install(applicationDefinition);
        }

        public UninstallationResult ForceUninstall(ApplicationDefinition applicationDefinition)
        {
            Stop(applicationDefinition);
            return Uninstall(applicationDefinition);
        }

        public StartResult ForceStart(ApplicationDefinition applicationDefinition)
        {
            Stop(applicationDefinition);
            return Start(applicationDefinition);
        }


        public abstract bool ForceDeviceShutDown();
    }
}