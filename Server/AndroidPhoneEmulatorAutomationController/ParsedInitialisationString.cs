//  ----------------------------------------------------------------------
//  <copyright file="ParsedInitialisationString.cs" company="Expensify">
//      (c) Copyright Expensify. http://www.expensify.com
//      This source is subject to the Microsoft Public License (Ms-PL)
//      Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//      All other rights reserved.
//  </copyright>
//  
//  Author - Stuart Lodge, Cirrious. http://www.cirrious.com
//  ------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace WindowsPhoneTestFramework.Server.AutomationController.Android.Emulator
{
    public class ParsedInitialisationString
    {
        public Dictionary<string, string> Fields { get; private set; }

        public ParsedInitialisationString(string initialisation)
        {
            FillFrom(initialisation);
        }

        public string SafeGetValue(string key, string defaultValue = "")
        {
            string toReturn;
            if (Fields.TryGetValue(key, out toReturn))
                return toReturn;

            return defaultValue;
        }

        public void FillFrom(string initialisation)
        {
            Fields = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(initialisation))
                return;

            var split = initialisation.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries);
            foreach (var entry in split)
            {
                var entrySplit = entry.Split(new[] {'='}, 2);
                if (entrySplit.Length != 2)
                    continue;

                var key = entrySplit[0];
                var value = entrySplit[1];
                if (string.IsNullOrEmpty(key))
                    continue;

                Fields[key] = value;
            }
        }
    }
}