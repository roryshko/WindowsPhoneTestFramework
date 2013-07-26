//  ----------------------------------------------------------------------
//  <copyright file="StepArgumentTransformers.cs" company="Expensify">
//      (c) Copyright Expensify. http://www.expensify.com
//      This source is subject to the Microsoft Public License (Ms-PL)
//      Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//      All other rights reserved.
//  </copyright>
//  
//  Author - Stuart Lodge, Cirrious. http://www.cirrious.com
//  ------------------------------------------------------------------------

using TechTalk.SpecFlow;

namespace WindowsPhoneTestFramework.Test.EmuSteps.StepDefinitions
{
    [Binding]
    public class StepArgumentTransformers
    {
        [StepArgumentTransformation(@"(\d.. |)")]
        public int OrdinalToIndexTransformer(string ordinal)
        {
            int index = 0;
            if (!string.IsNullOrWhiteSpace(ordinal))
            {
                // removes rd, st, nd, th or any other two letters

                ordinal = ordinal.Trim();

                if (int.TryParse(ordinal.Substring(0, ordinal.Length - 2), out index))
                    index--;
            }

            return index;
        }
    }
}