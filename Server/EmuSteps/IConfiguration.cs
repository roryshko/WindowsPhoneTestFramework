// ----------------------------------------------------------------------
// <copyright file="IConfiguration.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using System;
using WindowsPhoneTestFramework.Server.Core;

namespace WindowsPhoneTestFramework.Server.EmuSteps
{
    public interface IConfiguration
    {
        string AutomationControllerName { get; }
        string ControllerInitialisationString { get; }
        AutomationIdentification AutomationIdentification { get; }
        ApplicationDefinition ApplicationDefinition { get; }
    }
}