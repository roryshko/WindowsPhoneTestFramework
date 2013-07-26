//  ----------------------------------------------------------------------
//  <copyright file="MonoHacks.cs" company="Expensify">
//      (c) Copyright Expensify. http://www.expensify.com
//      This source is subject to the Microsoft Public License (Ms-PL)
//      Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//      All other rights reserved.
//  </copyright>
//  
//  Author - Stuart Lodge, Cirrious. http://www.cirrious.com
//  ------------------------------------------------------------------------

using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using WindowsPhoneTestFramework.Server.WCFHostedAutomationController.Results;
using WindowsPhoneTestFramework.Server.WCFHostedAutomationController.Utils;

namespace WindowsPhoneTestFramework.Server.WCFHostedAutomationController.Service
{
    public static class MonoHacks
    {
        // TODO - Would be "more than nice" to have a test harness on this class
        public static ResultBase DeserialiseResultBase(string json)
        {
            var regexId = new Regex("__type\\\":\\\"(?<one>[^:]*):#(?<two>[^\\\"]*)\"");
            var matchId = regexId.Match(json);
            if (!matchId.Success)
                throw new FormatException("Failed to decoded result - missing __type field");

            var className = matchId.Groups[1].Value;
            var assemblyName = matchId.Groups[2].Value;

            var type = Type.GetType(assemblyName + "." + className);
            var dcs = new DataContractJsonSerializer(type, KnownTypeProvider.GetKnownTypesFor(typeof (ResultBase)));

            var textStream = new MemoryStream(Encoding.Default.GetBytes(json));
            return (ResultBase) dcs.ReadObject(textStream);
        }
    }
}