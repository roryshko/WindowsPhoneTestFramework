//  ----------------------------------------------------------------------
//  <copyright file="CommandLine.cs" company="Expensify">
//      (c) Copyright Expensify. http://www.expensify.com
//      This source is subject to the Microsoft Public License (Ms-PL)
//      Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//      All other rights reserved.
//  </copyright>
//  
//  Author - Stuart Lodge, Cirrious. http://www.cirrious.com
//  ------------------------------------------------------------------------

using System.ComponentModel;
using Args;
using WindowsPhoneTestFramework.Server.Core;

namespace WindowsPhoneTestFramework.CommandLine.AutomationHost
{
    public class CommandLine
    {
        [ArgsMemberSwitch("bind")]
        [DefaultValue("http://localhost:8085/phoneAutomation")]
        [Description("Url and port binding for the phone automation host")]
        public string Binding { get; set; }

        [ArgsMemberSwitch("id")]
        [DefaultValue(AutomationIdentification.TryEverything)]
        [Description("Url and port binding for the phone automation host")]
        public AutomationIdentification AutomationIdentification { get; set; }
    }
}