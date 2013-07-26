//  ----------------------------------------------------------------------
//  <copyright file="AutomationElementCommandBase.cs" company="Expensify">
//      (c) Copyright Expensify. http://www.expensify.com
//      This source is subject to the Microsoft Public License (Ms-PL)
//      Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//      All other rights reserved.
//  </copyright>
//  
//  Author - Stuart Lodge, Cirrious. http://www.cirrious.com
//  ------------------------------------------------------------------------

using System.Runtime.Serialization;
using WindowsPhoneTestFramework.Server.WCFHostedAutomationController.Interfaces;

namespace WindowsPhoneTestFramework.Server.WCFHostedAutomationController.Commands
{
    [DataContract]
    public class AutomationElementCommandBase : CommandBase
    {
        [DataMember]
        public AutomationIdentifier AutomationIdentifier { get; set; }

        [DataMember]
        public int Ordinal { get; set; }

        [DataMember]
        public AutomationIdentifier ParentIdentifier { get; set; }
    }
}