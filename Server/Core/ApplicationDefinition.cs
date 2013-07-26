//  ----------------------------------------------------------------------
//  <copyright file="ApplicationDefinition.cs" company="Expensify">
//      (c) Copyright Expensify. http://www.expensify.com
//      This source is subject to the Microsoft Public License (Ms-PL)
//      Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//      All other rights reserved.
//  </copyright>
//  
//  Author - Stuart Lodge, Cirrious. http://www.cirrious.com
//  ------------------------------------------------------------------------

using System.Collections.Generic;

namespace WindowsPhoneTestFramework.Server.Core
{
    public class ApplicationDefinition
    {
        public Dictionary<string, string> Fields { get; set; }

        public ApplicationDefinition()
        {
            Fields = new Dictionary<string, string>();
        }
    }
}