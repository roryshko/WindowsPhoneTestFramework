// ----------------------------------------------------------------------
// <copyright file="ParsedObject.cs" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WindowsPhoneTestFramework.Server.DeviceController
{
    public abstract class ParsedObject
    {
        public abstract string ParsePrefix { get; }

        protected ParsedObject(IDictionary<string, string> fields)
        {
            FillStringProperties(fields);
        }

        public void FillStringProperties(IDictionary<string, string> fields)
        {
            var properties = GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
                .Where(p => p.CanRead && p.CanWrite)
                .Where(p => p.PropertyType == typeof(string));

            foreach (var property in properties)
            {
                string value;
                if (fields.TryGetValue(ParsePrefix + property.Name, out value))
                    property.SetValue(this, value, null);
            }
        }

        protected bool AreAllStringPropertiesFilled()
        {
            var nullStringProperties = NonFilledStringPropertyNames();
            return !nullStringProperties.Any();
        }

        protected IEnumerable<string> NonFilledStringPropertyNames()
        {
            var nullStringProperties = GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
                .Where(p => p.CanRead && p.CanWrite)
                .Where(p => p.PropertyType == typeof(string))
                .Where(p => null == p.GetValue(this, null))
                .Select(p => ParsePrefix + p.Name);

            return nullStringProperties;
        }
    }
}