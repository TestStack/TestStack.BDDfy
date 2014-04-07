using System;
using System.Collections.Generic;
using System.Linq;

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

        public IEnumerable<Story> CreateMixContainingEachTypeOfOutcome()
        {
            var storyMetadata1 = new StoryMetadata(typeof(RegularAccountHolderStory), "As a person", "I want ice cream", "So that I can be happy", "Happiness");
            var storyMetadata2 = new StoryMetadata(typeof(GoldAccountHolderStory), "As an account holder", "I want to withdraw cash", "So that I can get money when the bank is closed", "Account holder withdraws cash");
            
            var stories = new List<Story>()
            {
                new Story(storyMetadata1, GetOneOfEachScenarioResult()),
                new Story(storyMetadata2, GetOneOfEachScenarioResult())                
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

        private Scenario[] GetOneOfEachScenarioResult()
        {
            var scenarios = new List<Scenario>()
            {
                new Scenario(typeof(HappyPathScenario), GetHappyExecutionSteps(), "Happy Path Scenario"),
                new Scenario(typeof(SadPathScenario), GetSadExecutionSteps(), "Sad Path Scenario"),
                new Scenario(typeof(SadPathScenario), GetInconclusiveExecutionSteps(), "Inconclusive Scenario"),
                new Scenario(typeof(SadPathScenario), GetNotImplementedExecutionSteps(), "Not Implemented Scenario")
            };

            // override specific step results - ideally this class could be refactored to provide  objectmother/builder interface
            SetAllStepResults(scenarios[0].Steps, Result.Passed);

            SetAllStepResults(scenarios[1].Steps, Result.Passed);
            scenarios[1].Steps.Last().Result = Result.Failed;

            return scenarios.ToArray();
        }

        private IEnumerable<Step> GetHappyExecutionSteps()
        {
            var steps = new List<Step>()
            {
                new Step(null, "Given a positive account balance", true, ExecutionOrder.Assertion, true) {Duration = new TimeSpan(0, 0, 0, 0, 5)},
                new Step(null, "When the account holder requests money", true, ExecutionOrder.Assertion, true) {Duration = new TimeSpan(0, 0, 0, 0, 5)},
                new Step(null, "Then money is dispensed", true, ExecutionOrder.Assertion, true) {Duration = new TimeSpan(0, 0, 0, 0, 5)},
            };
            return steps;
        }

        private IEnumerable<Step> GetSadExecutionSteps()
        {
            var steps = new List<Step>()
            {
                new Step(null, "Given a negative account balance", true, ExecutionOrder.Assertion, true) {Duration = new TimeSpan(0, 0, 0, 0, 5)},
                new Step(null, "When the account holder requests money", true, ExecutionOrder.Assertion, true) {Duration = new TimeSpan(0, 0, 0, 0, 5)},
                new Step(null, "Then no money is dispensed", true, ExecutionOrder.Assertion, true) {Duration = new TimeSpan(0, 0, 0, 0, 5)},
            };
            return steps;
        }

        private IEnumerable<Step> GetInconclusiveExecutionSteps()
        {
            var steps = new List<Step>()
            {
                new Step(null, "Given a negative account balance", true, ExecutionOrder.Assertion, true) {Duration = new TimeSpan(0, 0, 0, 0, 5)},
                new Step(null, "When the account holder requests money", true, ExecutionOrder.Assertion, true) {Duration = new TimeSpan(0, 0, 0, 0, 5)},
                new Step(null, "Then no money is dispensed", true, ExecutionOrder.Assertion, true) {Duration = new TimeSpan(0, 0, 0, 0, 5)},
            };

            SetAllStepResults(steps, Result.Passed);

            steps.Last().Result = Result.Inconclusive;

            return steps;
        }


        private IEnumerable<Step> GetNotImplementedExecutionSteps()
        {
            var steps = new List<Step>()
            {
                new Step(null, "Given a negative account balance", true, ExecutionOrder.Assertion, true) {Duration = new TimeSpan(0, 0, 0, 0, 5)},
                new Step(null, "When the account holder requests money", true, ExecutionOrder.Assertion, true) {Duration = new TimeSpan(0, 0, 0, 0, 5)},
                new Step(null, "Then no money is dispensed", true, ExecutionOrder.Assertion, true) {Duration = new TimeSpan(0, 0, 0, 0, 5)},
            };

            SetAllStepResults(steps, Result.Passed);

            steps.Last().Result = Result.NotImplemented;

            return steps;
        }

        private void SetAllStepResults(IEnumerable<Step> steps, Result result)
        {
            foreach (var step in steps)
            {
                step.Result = result;
            }
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