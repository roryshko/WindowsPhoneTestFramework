//  ----------------------------------------------------------------------
//  <copyright file="IPhoneAutomationServiceControl.cs" company="Expensify">
//      (c) Copyright Expensify. http://www.expensify.com
//      This source is subject to the Microsoft Public License (Ms-PL)
//      Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//      All other rights reserved.
//  </copyright>
//  
//  Author - Stuart Lodge, Cirrious. http://www.cirrious.com
//  ------------------------------------------------------------------------

using System;
using WindowsPhoneTestFramework.Server.WCFHostedAutomationController.Commands;
using WindowsPhoneTestFramework.Server.WCFHostedAutomationController.Results;

namespace WindowsPhoneTestFramework.Server.WCFHostedAutomationController.Interfaces
{
    public interface IPhoneAutomationServiceControl
    {
        void AddCommand(CommandBase command, Action<ResultBase> onResult);

        void AddCommand(CommandBase command, Action<ResultBase> onResult, TimeSpan sendCommandWithin,
                        TimeSpan expectResultWithin);

        void Clear();
    }
}