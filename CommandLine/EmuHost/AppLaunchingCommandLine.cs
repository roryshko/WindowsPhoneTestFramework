// ----------------------------------------------------------------------
// <copyright file="AppLaunchingCommandLine.cs" company="Expensify">
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
using Args;
using WindowsPhoneTestFramework.Server.Core;

namespace WindowsPhoneTestFramework.CommandLine.EmuHost
{
    [Description("emuhost - provides communications with the Windows Phone Emulator")]
    public class AppLaunchingCommandLine
    {
        [ArgsMemberSwitch("controller")]
        [DefaultValue("WindowsPhoneTestFramework.Server.AutomationController.WindowsPhone.Emulator.dll")]
        [Description("Assembly to use as controller - by default this is WindowsPhoneTestFramework.Server.AutomationController.WindowsPhone.Emulator.dll")]
        public string Controller { get; set; }

        [ArgsMemberSwitch("init")]
        [DefaultValue("")]
        [Description("Initialisation string for the contoller")]
        public string Initialisation { get; set; }

        [ArgsMemberSwitch("autoid")]
        [DefaultValue(AutomationIdentification.TryEverything)]
        [Description("Mechanism for identifying phone controls - defaults to TryEverything")]
        public AutomationIdentification AutomationIdentification { get; set; }

        // Windows Phone specific fields
        [ArgsMemberSwitch("wppid")]
        [Description("The WindowsPhone Product Id (Guid)")]
        [ApplicationDefinitionArg("WindowsPhone", "ApplicationId")]
        public Guid WindowsPhoneProductId { get; set; }

        [ArgsMemberSwitch("wpicon")]
        [DefaultValue("ApplicationIcon.png")]
        [Description("Path to WindowsPhone application icon file (62x62 .png)")]
        [ApplicationDefinitionArg("WindowsPhone", "ApplicationIconPath")]
        public string WindowsPhoneIconPath { get; set; }

        [ArgsMemberSwitch("wpxap")]
        [Description("Path to WindowsPhone application package file - .xap")]
        [ApplicationDefinitionArg("WindowsPhone", "ApplicationPackagePath")]
        public string WindowsPhonePackagePath { get; set; }

        [ArgsMemberSwitch("wpname")]
        [DefaultValue("Test Application")]
        [Description("WindowsPhone application name")]
        [ApplicationDefinitionArg("WindowsPhone", "ApplicationName")]
        public string WindowsPhoneName { get; set; }

        // Android specific fields
        [ArgsMemberSwitch("andapk")]
        [Description("Path to Android package file - .apk")]
        [ApplicationDefinitionArg("Android", "PackagePath")]
        public string AndroidPackagePath { get; set; }

        [ArgsMemberSwitch("andpackage")]
        [Description("Android package name - e.g. 'com.acme.mypackage'")]
        [ApplicationDefinitionArg("Android", "PackageName")]
        public string AndroidPackageName { get; set; }

        [ArgsMemberSwitch("andtestapk")]
        [Description("Path to Android JUnit test stub package file - .apk")]
        [ApplicationDefinitionArg("Android", "StubPackagePath")]
        public string AndroidStubPackagePath { get; set; }

        [ArgsMemberSwitch("andtestpackage")]
        [Description("Android JUnit test stub package name - e.g. 'com.acme.mypackage.test'")]
        [ApplicationDefinitionArg("Android", "StubPackageName")]
        public string AndroidStubPackageName { get; set; }

        // remobe this?
        [ArgsMemberSwitch("andaction")]
        [Description("Android action to start - e.g. android.intent.action.MAIN")]
        [ApplicationDefinitionArg("Android", "Action")]
        [DefaultValue("android.intent.action.MAIN")]
        public string AndroidAction { get; set; }

        // remobe this?
        [ArgsMemberSwitch("andclass")]
        [Description("Android activity Class name - e.g. 'com.acme.mypackage.Program'")]
        [ApplicationDefinitionArg("Android", "ActivityClassName")]
        public string ActivityClassName { get; set; }
    }
}