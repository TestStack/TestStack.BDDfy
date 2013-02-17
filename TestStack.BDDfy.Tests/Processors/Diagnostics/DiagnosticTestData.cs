using System;
using System.Collections.Generic;
using TestStack.BDDfy.Core;

namespace TestStack.BDDfy.Tests.Processors.Diagnostics
{
    public class DiagnosticTestData
    {
        public IEnumerable<Core.Story> CreateTwoStoriesEachWithTwoScenariosWithTwoStepsOfFiveMilliseconds()
        {
            var storyMetaData1 = new StoryMetaData(typeof(DummyStory1), "As a person", "I want ice cream", "So that I can be happy", "Happiness");
            var storyMetaData2 = new StoryMetaData(typeof(DummyStory2), "As an account holder", "I want to withdraw cash", "So that I can get money when the bank is closed", "Account holder withdraws cash");
            var stories = new List<Core.Story>()
            {
                new Core.Story(storyMetaData1, GetScenarios()),
                new Core.Story(storyMetaData2, GetScenarios())
            };

            return stories;
        }

        private Scenario[] GetScenarios()
        {
            var scenarios = new List<Scenario>()
            {
                new Scenario(typeof(DummyScenario1), GetExecutionSteps(), "scenario1"),
                new Scenario(typeof(DummyScenario2), GetExecutionSteps(), "scenario2")
            };
            return scenarios.ToArray();
        }

        private IEnumerable<ExecutionStep> GetExecutionSteps()
        {
            var steps = new List<ExecutionStep>()
            {
                new ExecutionStep(null, null, true, ExecutionOrder.Assertion, true) {Duration = new TimeSpan(0, 0, 0, 0, 5)},
                new ExecutionStep(null, null, true, ExecutionOrder.Assertion, true) {Duration = new TimeSpan(0, 0, 0, 0, 5)},
            };
            return steps;
        }

        public class DummyStory1 { }
        public class DummyStory2 { }
        public class DummyScenario1 { }
        public class DummyScenario2 { }
    }
}