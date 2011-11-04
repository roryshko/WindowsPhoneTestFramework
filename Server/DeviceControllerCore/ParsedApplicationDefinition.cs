// ----------------------------------------------------------------------
// <copyright file="ParsedApplicationDefinition.cs" company="Expensify">
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

namespace WindowsPhoneTestFramework.Server.DeviceController
{
    public abstract class ParsedApplicationDefinition : ParsedObject
    {
        protected ParsedApplicationDefinition(ApplicationDefinition applicationDefinition)
            : base(applicationDefinition.Fields)
        {
        }
    }
}