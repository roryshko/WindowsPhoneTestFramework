﻿//  ----------------------------------------------------------------------
//  <copyright file="ProgressResult.cs" company="Expensify">
//      (c) Copyright Expensify. http://www.expensify.com
//      This source is subject to the Microsoft Public License (Ms-PL)
//      Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//      All other rights reserved.
//  </copyright>
//  
//  Author - Stuart Lodge, Cirrious. http://www.cirrious.com
//  ------------------------------------------------------------------------

using System.Runtime.Serialization;

namespace WindowsPhoneTestFramework.Server.WCFHostedAutomationController.Results
{
    [DataContract]
    public class ProgressResult : ResultBase
    {
        [DataMember]
        public double Min { get; set; }

        [DataMember]
        public double Max { get; set; }

        [DataMember]
        public double Current { get; set; }
    }
}