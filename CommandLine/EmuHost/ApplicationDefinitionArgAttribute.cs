//  ----------------------------------------------------------------------
//  <copyright file="ApplicationDefinitionArgAttribute.cs" company="Expensify">
//      (c) Copyright Expensify. http://www.expensify.com
//      This source is subject to the Microsoft Public License (Ms-PL)
//      Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//      All other rights reserved.
//  </copyright>
//  
//  Author - Stuart Lodge, Cirrious. http://www.cirrious.com
//  ------------------------------------------------------------------------

using System;

namespace WindowsPhoneTestFramework.CommandLine.EmuHost
{
    public class ApplicationDefinitionArgAttribute : Attribute
    {
        public string Prefix { get; set; }
        public string Name { get; set; }

        public string FullName
        {
            get { return Prefix + Name; }
        }

        public ApplicationDefinitionArgAttribute(string prefix, string name)
        {
            prefix = prefix.Trim();

            if (prefix.Length > 0 && !prefix.EndsWith("."))
                prefix = prefix + ".";

            Prefix = prefix;
            Name = name;
        }
    }
}