using System;
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
            int actualValue = 0;
            var exampleTable = this.WithExamples(new ExampleTable("ExampleValue")
            {
                expectedValue
            }).Single();

            var sut = new TestRunner();
            Action<object> action = o => actualValue = ExampleValue;
            var steps = new[]{new Step(action, "A Step", true, ExecutionOrder.Initialize, true) };
            
            var scenarioWithExample = new Scenario("id", this, steps, "Scenario Text", exampleTable);
            var story = new Story(new StoryMetadata(typeof(TestRunnerTests), new StoryNarrativeAttribute()),
                new[]{ scenarioWithExample});

            sut.Process(story);

            Assert.AreEqual(expectedValue, actualValue);
        }
    }
}