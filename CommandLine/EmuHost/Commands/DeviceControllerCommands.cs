// ----------------------------------------------------------------------
// <copyright file="DeviceControllerCommands.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using WindowsPhoneTestFramework.CommandLine.CommandLineHost;
using WindowsPhoneTestFramework.Server.Core;

namespace WindowsPhoneTestFramework.CommandLine.EmuHost.Commands
{
    public class DeviceControllerCommands
    {
        public IDeviceController DeviceController { get; set; }
        public AppLaunchingCommandLine CommandLine { get; set; }

        private ApplicationDefinition CurrentApplicationDefinition
        {
            get 
            { 
                var toReturn = new ApplicationDefinition();

                var properties =
                    from property in CommandLine.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty)
                    let argAttribute =
                        (ApplicationDefinitionArgAttribute)property.GetCustomAttributes(typeof (ApplicationDefinitionArgAttribute), false).FirstOrDefault()
                    where argAttribute != null
                    let propertyValue = property.GetValue(CommandLine, null)
                    where propertyValue != null
                    select new
                               {
                                   StringValue = propertyValue.ToString(),
                                   FullName = argAttribute.FullName
                               };

                foreach (var property in properties)
                    toReturn.Fields[property.FullName] = property.StringValue;

                return toReturn;
            }
        }

        [CommandLineCommand("install")]
        [Description("installs the app - e.g. 'install'")]
        public void Install(string ignored)
        {
            var result = DeviceController.Install(CurrentApplicationDefinition);
            Console.WriteLine("install:" + result);
        }

        [CommandLineCommand("forceInstall")]
        [Description("installs the app - shutting it down first if required - e.g. 'forceInstall'")]
        public void ForceInstall(string ignored)
        {
            var result = DeviceController.ForceInstall(CurrentApplicationDefinition);
            Console.WriteLine("forceInstall:" + result);
        }

        [CommandLineCommand("uninstall")]
        [Description("uninstalls the app - e.g. 'uninstall'")]
        public void Uninstall(string ignored)
        {
            var result = DeviceController.Uninstall(CurrentApplicationDefinition);
            Console.WriteLine("uninstall:" + result);
        }

        [CommandLineCommand("forceUninstall")]
        [Description("uninstalls the app - shutting it down first if required - e.g. 'forceUninstall'")]
        public void ForceUninstall(string ignored)
        {
            var result = DeviceController.ForceUninstall(CurrentApplicationDefinition);
            Console.WriteLine("forceUninstall:" + result);
        }

        [CommandLineCommand("launch")]
        [Description("launches the app - e.g. 'launch'")]
        public void Launch(string ignored)
        {
            var result = DeviceController.Start(CurrentApplicationDefinition);
            Console.WriteLine("launch:" + result);
        }

        [CommandLineCommand("stop")]
        [Description("stop the app - e.g. 'stop'")]
        public void Stop(string ignored)
        {
            var result = DeviceController.Stop(CurrentApplicationDefinition);
            Console.WriteLine("stop:" + result);
        }
    }
}