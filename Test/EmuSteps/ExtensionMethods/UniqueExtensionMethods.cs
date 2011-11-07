using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace WindowsPhoneTestFramework.Test.EmuSteps.ExtensionMethods
{
    public static class UniqueExtensionMethods
    {
        public const string UniqueContextPrefix = "unique-";
        public const string UniqueTableSyntaxPrefix = "$unique:";

        public static void StoreAsNamedUnique(this object toStore, string uniqueKey)
        {
            ScenarioContext.Current["unique-" + uniqueKey] = toStore;
        }

        public static string ReplaceUniqueKey(this string uniqueKey)
        {
            object uniqueValue;
            Assert.IsTrue(ScenarioContext.Current.TryGetValue(UniqueContextPrefix + uniqueKey, out uniqueValue), "Unique Value is missing: " + uniqueKey);
            Assert.IsNotNull(uniqueValue, "Unique value is null: " + uniqueKey);
            return uniqueValue.ToString();
        }

        public static string ReplaceUniqueTokenIfNecessary(this string candidateText)
        {
            // Note - this replacement mechanism doesn't allow any escaping or other special code
            //    - if any users genuinely need to start text with "$unique:", then you'll need a different mechanism!
            if (string.IsNullOrEmpty(candidateText))
                return candidateText;

            if (!candidateText.StartsWith(UniqueTableSyntaxPrefix))
            {
                return candidateText;
            }

            var uniqueKey = candidateText.Substring(UniqueTableSyntaxPrefix.Length);
            return uniqueKey.ReplaceUniqueKey();
        }
    }
}
