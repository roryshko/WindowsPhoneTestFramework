using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
