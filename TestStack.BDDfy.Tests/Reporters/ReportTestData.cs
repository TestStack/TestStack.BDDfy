using System;
using System.Collections.Generic;

namespace TestStack.BDDfy.Tests.Reporters
{
    public class ReportTestData
    {
        public IEnumerable<Story> CreateTwoStoriesEachWithTwoScenariosWithThreeStepsOfFiveMilliseconds()
        {
            var storyMetadata1 = new StoryMetadata(typeof(RegularAccountHolderStory), "As a person", "I want ice cream", "So that I can be happy", "Happiness");
            var storyMetadata2 = new StoryMetadata(typeof(GoldAccountHolderStory), "As an account holder", "I want to withdraw cash", "So that I can get money when the bank is closed", "Account holder withdraws cash");
            var stories = new List<Story>()
            {
                new Story(storyMetadata1, GetScenarios()),
                new Story(storyMetadata2, GetScenarios())
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

        private IEnumerable<Step> GetHappyExecutionSteps()
        {
            var steps = new List<Step>
            {
                new Step(null, "Given a positive account balance", true, ExecutionOrder.Assertion, true) {Duration = new TimeSpan(0, 0, 0, 0, 5)},
                new Step(null, "When the account holder requests money", true, ExecutionOrder.Assertion, true) {Duration = new TimeSpan(0, 0, 0, 0, 5)},
                new Step(null, "Then money is dispensed", true, ExecutionOrder.Assertion, true) {Duration = new TimeSpan(0, 0, 0, 0, 5)},
            };
            return steps;
        }

        private IEnumerable<Step> GetSadExecutionSteps()
        {
            var steps = new List<Step>
            {
                new Step(null, "Given a negative account balance", true, ExecutionOrder.Assertion, true) {Duration = new TimeSpan(0, 0, 0, 0, 5)},
                new Step(null, "When the account holder requests money", true, ExecutionOrder.Assertion, true) {Duration = new TimeSpan(0, 0, 0, 0, 5)},
                new Step(null, "Then no money is dispensed", true, ExecutionOrder.Assertion, true) {Duration = new TimeSpan(0, 0, 0, 0, 5)},
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