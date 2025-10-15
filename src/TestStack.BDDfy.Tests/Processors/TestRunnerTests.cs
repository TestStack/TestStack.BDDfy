using System;
using System.Collections.Generic;
using System.Linq;
using Shouldly;
using TestStack.BDDfy.Processors;
using Xunit;

namespace TestStack.BDDfy.Tests.Processors
{
    public class TestRunnerTests
    {
        public int ExampleValue { get; set; }

        [Fact]
        public void InitializesScenarioWithExampleBeforeRunning()
        {
            const int expectedValue = 1;
            var actualValue = 0;
            var exampleTable = new ExampleTable("ExampleValue")
            {
                expectedValue
            }.Single();

            var sut = new TestRunner();
            Func<object, object> action = o => actualValue = ExampleValue;
            var steps = new List<Step> { new(action, new StepTitle("A Step"), true, ExecutionOrder.Initialize, true, new List<StepArgument>()) };

            var scenarioWithExample = new Scenario("id", this, steps, "Scenario Text", exampleTable, new List<string>());
            var story = new Story(new StoryMetadata(typeof(TestRunnerTests), new StoryNarrativeAttribute()), scenarioWithExample);

            sut.Process(story);

            actualValue.ShouldBe(expectedValue);
        }
    }
}