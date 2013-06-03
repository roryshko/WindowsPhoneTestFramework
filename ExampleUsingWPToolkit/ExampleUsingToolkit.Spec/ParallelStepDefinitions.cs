using NUnit.Framework;

using TechTalk.SpecFlow;

namespace ExampleUsingToolkit.Spec
{
    [Binding]
    public class ParallelStepDefinitions
    {
        private const string ConstantKey = "TestThingamy";

        [Given(@"I store the value ""(.*)"" in the scenario context")]
        public void GiveIStoreAValueInTheContext(string value)
        {
            ScenarioContext.Current.Add(ConstantKey, value);
        }

        [Then(@"I can read the value ""(.*)"" from the scenario context")]
        public void ThenTheResultShouldBe(string value)
        {
            Assert.IsTrue(ScenarioContext.Current.ContainsKey(ConstantKey));

            Assert.AreEqual(ScenarioContext.Current[ConstantKey], value);
        }
    }
}
