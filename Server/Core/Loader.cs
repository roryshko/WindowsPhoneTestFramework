// ----------------------------------------------------------------------
// <copyright file="Loader.cs" company="Expensify">
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
using System.Reflection;

namespace WindowsPhoneTestFramework.Server.Core
{
    public static class Loader
    {
        public static IAutomationController LoadFrom(string assemblyNameOrNickName)
        {
            string assemblyNameToLoad = GetAssemblyNameToLoad(assemblyNameOrNickName);
            return LoadFromAssemblyFileName(assemblyNameToLoad);
        }

        private static string GetAssemblyNameToLoad(string assemblyNameOrNickName)
        {
            string assemblyNameToLoad;
            switch (assemblyNameOrNickName.ToLower())
            {
                case "wp":
                case "wp7":
                case "wp7-emulator":
                case "wp7emulator":
                case "wp-emulator":
                case "wpemulator":
                    assemblyNameToLoad = "WindowsPhoneTestFramework.Server.AutomationController.WindowsPhone.Emulator.dll";
                    break;
                case "android":
                case "droid":
                case "android-emulator":
                case "droid-emulator":
                    assemblyNameToLoad = "WindowsPhoneTestFramework.Server.AutomationController.Android.Emulator.dll";
                    break;
                default:
                    assemblyNameToLoad = assemblyNameOrNickName;
                    break;
            }

            // technically .dll doesn't cover all assemblies, but it's good enough for now
            if (!assemblyNameToLoad.EndsWith(".dll") && !assemblyNameToLoad.EndsWith(".dll"))
                assemblyNameToLoad = assemblyNameToLoad + ".dll";
            return assemblyNameToLoad;
        }

        public static IAutomationController LoadFromAssemblyFileName(string assemblyName)
        {
            try
            {
                var assembly = Assembly.LoadFrom(assemblyName);
                var type = assembly
                    .GetTypes()
                    .Where(t => t.IsPublic)
                    .FirstOrDefault(t => t.GetInterfaces().Any(x => x == typeof (IAutomationController)));
                
                if (type == null)
                    throw new TypeLoadException("no Controller type found in assembly");

                var constructor = type.GetConstructors().First(x => x.GetParameters().Count() == 0);

                var toReturn = constructor.Invoke(new object[0]);
                return (IAutomationController) toReturn;
            }
            catch (Exception exception)
            {                
                throw new AutomationException("Failed to create automation controller ", exception);
            }            
        }
    }
}