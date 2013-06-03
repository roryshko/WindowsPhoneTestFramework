// ----------------------------------------------------------------------
// <copyright file="SuccessResult.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using WindowsPhoneTestFramework.Server.Core;

namespace WindowsPhoneTestFramework.Server.WCFHostedAutomationController.Results
{
    [DataContract]
    public class DictionaryResult : ResultBase
    {
        [DataMember]
        public Dictionary<string, string> Results { get; set; }
    }
}