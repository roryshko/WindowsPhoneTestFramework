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

using System.Runtime.Serialization;

namespace WindowsPhoneTestFramework.Server.WCFHostedAutomationController.Results
{
    using System.Drawing;

    using WindowsPhoneTestFramework.Server.Core.Types;

    [DataContract]
    public class SuccessResultColor : ResultBase
    {
        [DataMember]
        public string ResultColor { get; set; }
    }
}