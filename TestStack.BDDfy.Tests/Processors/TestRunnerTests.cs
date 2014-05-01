using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TestStack.BDDfy.Processors;

namespace TestStack.BDDfy.Tests.Processors
{
    [TestFixture]
    public class TestRunnerTests
    {
        public int ExampleValue { get; set; }

        [Test]
        public void InitialisesScenarioWithExampleBeforeRunning()
        {
            const int ExpectedValue = 1;
            int actualValue = 0;
            var exampleTable = new ExampleTable("ExampleValue")
            {
                ExpectedValue
            }.Single();

            var sut = new TestRunner();
            Action<object> action = o => actualValue = ExampleValue;
            var steps = new List<Step> { new Step(action, new StepTitle("A Step"), true, ExecutionOrder.Initialize, true) };

            var scenarioWithExample = new Scenario("id", this, steps, true, "Scenario Text", exampleTable);
            var story = new Story(new StoryMetadata(typeof(TestRunnerTests), new StoryNarrativeAttribute()),
                new[] { scenarioWithExample });

            sut.Process(story);

            Assert.AreEqual(ExpectedValue, actualValue);
        }
    }
}