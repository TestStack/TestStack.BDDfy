using System;
using System.Collections.Generic;
using TestStack.BDDfy.Core;

namespace TestStack.BDDfy.Tests.Processors.Reports
{
    public class ReportTestData
    {
        public IEnumerable<Core.Story> CreateTwoStoriesEachWithTwoScenariosWithThreeStepsOfFiveMilliseconds()
        {
            var storyMetaData1 = new StoryMetaData(typeof(RegularAccountHolderStory), "As a person", "I want ice cream", "So that I can be happy", "Happiness");
            var storyMetaData2 = new StoryMetaData(typeof(GoldAccountHolderStory), "As an account holder", "I want to withdraw cash", "So that I can get money when the bank is closed", "Account holder withdraws cash");
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
                new Scenario(typeof(HappyPathScenario), GetHappyExecutionSteps(), "Happy Path Scenario"),
                new Scenario(typeof(SadPathScenario), GetSadExecutionSteps(), "Sad Path Scenario")
            };
            return scenarios.ToArray();
        }

        private IEnumerable<ExecutionStep> GetHappyExecutionSteps()
        {
            var steps = new List<ExecutionStep>()
            {
                new ExecutionStep(null, "Given a positive account balance", true, ExecutionOrder.Assertion, true) {Duration = new TimeSpan(0, 0, 0, 0, 5)},
                new ExecutionStep(null, "When the account holder requests money", true, ExecutionOrder.Assertion, true) {Duration = new TimeSpan(0, 0, 0, 0, 5)},
                new ExecutionStep(null, "Then money is dispensed", true, ExecutionOrder.Assertion, true) {Duration = new TimeSpan(0, 0, 0, 0, 5)},
            };
            return steps;
        }

        private IEnumerable<ExecutionStep> GetSadExecutionSteps()
        {
            var steps = new List<ExecutionStep>()
            {
                new ExecutionStep(null, "Given a negative account balance", true, ExecutionOrder.Assertion, true) {Duration = new TimeSpan(0, 0, 0, 0, 5)},
                new ExecutionStep(null, "When the account holder requests money", true, ExecutionOrder.Assertion, true) {Duration = new TimeSpan(0, 0, 0, 0, 5)},
                new ExecutionStep(null, "Then no money is dispensed", true, ExecutionOrder.Assertion, true) {Duration = new TimeSpan(0, 0, 0, 0, 5)},
            };
            return steps;
        }

        public class RegularAccountHolderStory { }
        public class GoldAccountHolderStory { }
        public class HappyPathScenario
        {
            public void GivenAPositiveAccountBalance() { }
            public void WhenTheAccountHolderRequestsMoney() { }
            public void ThenMoneyIsDispensed() { }
        }
        public class SadPathScenario
        {
            public void GivenANegativeAccountBalance() { }
            public void WhenTheAccountHolderRequestsMoney() { }
            public void ThenNoMoneyIsDispensed() { }
        }
    }
}