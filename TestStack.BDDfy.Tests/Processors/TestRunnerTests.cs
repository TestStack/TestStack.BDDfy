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
            const int expectedValue = 1;
            var actualValue = 0;
            var exampleTable = new ExampleTable("ExampleValue")
            {
                expectedValue
            }.Single();

            var sut = new TestRunner();
            Action<object> action = o => actualValue = ExampleValue;
            var steps = new List<Step> { new Step(action, new StepTitle("A Step"), true, ExecutionOrder.Initialize, true, new List<StepArgument>()) };

            var scenarioWithExample = new Scenario("id", this, steps, "Scenario Text", exampleTable, new List<string>());
            var story = new Story(new StoryMetadata(typeof(TestRunnerTests), new StoryNarrativeAttribute()),
                new[] { scenarioWithExample });

            sut.Process(story);

            Assert.AreEqual(expectedValue, actualValue);
        }
    }
}