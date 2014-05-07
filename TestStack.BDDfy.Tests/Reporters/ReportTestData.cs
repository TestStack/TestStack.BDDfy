using System;
using System.Collections.Generic;

namespace TestStack.BDDfy.Tests.Reporters
{
    using System.Linq;

    public class ReportTestData
    {
        private int _idCount;

        public IEnumerable<Story> CreateTwoStoriesEachWithOneFailingScenarioAndOnePassingScenarioWithThreeStepsOfFiveMilliseconds()
        {
            var storyMetadata1 = new StoryMetadata(typeof(RegularAccountHolderStory), "As a person", "I want ice cream", "So that I can be happy", "Happiness");
            var storyMetadata2 = new StoryMetadata(typeof(GoldAccountHolderStory), "As an account holder", "I want to withdraw cash", "So that I can get money when the bank is closed", "Account holder withdraws cash");
            var stories = new List<Story>
            {
                new Story(storyMetadata1, GetScenarios(false, false)),
                new Story(storyMetadata2, GetScenarios(true, false))
            };

            return stories;
        }

        public IEnumerable<Story> CreateMixContainingEachTypeOfOutcome()
        {
            var storyMetadata1 = new StoryMetadata(typeof(RegularAccountHolderStory), "As a person", "I want ice cream", "So that I can be happy", "Happiness");
            var storyMetadata2 = new StoryMetadata(typeof(GoldAccountHolderStory), "As an account holder", "I want to withdraw cash", "So that I can get money when the bank is closed", "Account holder withdraws cash");

            const StoryMetadata testThatReportWorksWithNoStory = null;

            var stories = new List<Story>
            {
                new Story(storyMetadata1, GetOneOfEachScenarioResult()),
                new Story(storyMetadata2, GetOneOfEachScenarioResult()),
                new Story(testThatReportWorksWithNoStory, GetOneOfEachScenarioResult())
            };

            return stories;
        }

        public IEnumerable<Story> CreateMixContainingEachTypeOfOutcomeWithOneScenarioPerStory()
        {
            var storyMetadata1 = new StoryMetadata(typeof(RegularAccountHolderStory), "As a person", "I want ice cream", "So that I can be happy", "Happiness");
            var storyMetadata2 = new StoryMetadata(typeof(GoldAccountHolderStory), "As an unhappy examples story", "I want to see failed steps", "So that I can diagnose what's wrong", "Unhappy examples");
            var storyMetadata3 = new StoryMetadata(typeof(PlatinumAccountHolderStory), "As a happy examples story", "I want a clean report with examples", "So that the report is clean and readable", "Happy Examples");

            const StoryMetadata testThatReportWorksWithNoStory = null;

            var stories = new List<Story>
            {
                new Story(storyMetadata1, new Scenario(typeof(HappyPathScenario), GetHappyExecutionSteps(), "Happy Path Scenario [for Happiness]", new List<string>())),
                new Story(storyMetadata1, new Scenario(typeof(SadPathScenario), GetFailingExecutionSteps(), "Sad Path Scenario [for Happiness]", new List<string>())),
                new Story(storyMetadata1, new Scenario(typeof(SadPathScenario), GetInconclusiveExecutionSteps(), "Inconclusive Scenario [for Happiness]", new List<string>())),
                new Story(storyMetadata1, new Scenario(typeof(SadPathScenario), GetNotImplementedExecutionSteps(), "Not Implemented Scenario [for Happiness]", new List<string>())),
                new Story(testThatReportWorksWithNoStory, new Scenario(typeof(HappyPathScenario), GetHappyExecutionSteps(), "Happy Path Scenario [with no story]", new List<string>())),
                new Story(testThatReportWorksWithNoStory, new Scenario(typeof(SadPathScenario), GetFailingExecutionSteps(), "Sad Path Scenario [with no story]", new List<string>())),
                new Story(testThatReportWorksWithNoStory, new Scenario(typeof(SadPathScenario), GetInconclusiveExecutionSteps(), "Inconclusive Scenario [with no story]", new List<string>())),
                new Story(testThatReportWorksWithNoStory, new Scenario(typeof(SadPathScenario), GetNotImplementedExecutionSteps(), "Not Implemented Scenario [with no story]", new List<string>())),
                new Story(storyMetadata2, GetScenarios(true, true)),
                new Story(storyMetadata3, GetScenarios(false, true)),
            };

            return stories;
        }

        public IEnumerable<Story> CreateTwoStoriesEachWithOneFailingScenarioAndOnePassingScenarioWithThreeStepsOfFiveMillisecondsAndEachHasTwoExamples()
        {
            var storyMetadata1 = new StoryMetadata(typeof(RegularAccountHolderStory), "As a person", "I want ice cream", "So that I can be happy", "Happiness");
            var storyMetadata2 = new StoryMetadata(typeof(GoldAccountHolderStory), "As an account holder", "I want to withdraw cash", "So that I can get money when the bank is closed", "Account holder withdraws cash");
            var stories = new List<Story>
            {
                new Story(storyMetadata1, GetScenarios(false, true)),
                new Story(storyMetadata2, GetScenarios(true, true))
            };

            return stories;
        }

        private Scenario[] GetScenarios(bool includeFailingScenario, bool includeExamples)
        {
            var sadExecutionSteps = GetSadExecutionSteps().ToList();
            if (includeFailingScenario)
            {
                var last = sadExecutionSteps.Last();
                last.Result = Result.Failed;
                try
                {
                    throw new InvalidOperationException("Boom");
                }
                catch (Exception ex)
                {
                    last.Exception = ex;
                }
            }

            if (includeExamples)
            {
                var exampleId = _idCount++.ToString();
                var exampleTable = new ExampleTable("sign", "action")
                                       {
                                            {"positive", "is"},
                                            {"negative", "is not"}
                                       };
                var exampleExecutionSteps = GetExampleExecutionSteps().ToList();
                if (includeFailingScenario)
                {
                    var last = exampleExecutionSteps.Last();
                    last.Result = Result.Failed;
                    try
                    {
                        throw new InvalidOperationException("Boom\nWith\r\nNew lines");
                    }
                    catch (Exception ex)
                    {
                        last.Exception = ex;
                    }
                }
                return new List<Scenario>
                {
                    new Scenario(exampleId, typeof(ExampleScenario), GetExampleExecutionSteps(), "Example Scenario", exampleTable.ElementAt(0), new List<StepArgument>(), new List<string>()),
                    new Scenario(exampleId, typeof(ExampleScenario), exampleExecutionSteps, "Example Scenario", exampleTable.ElementAt(1), new List<StepArgument>(), new List<string>())
                }.ToArray();
            }

            return new List<Scenario>
                       {
                           new Scenario(typeof(HappyPathScenario), GetHappyExecutionSteps(), "Happy Path Scenario", new List<string>()),
                           new Scenario(typeof(SadPathScenario), sadExecutionSteps, "Sad Path Scenario", new List<string>())
                       }.ToArray();
        }

        private Scenario[] GetOneOfEachScenarioResult()
        {
            var scenarios = new List<Scenario>
            {
                new Scenario(typeof(HappyPathScenario), GetHappyExecutionSteps(), "Happy Path Scenario", new List<string>()),
                new Scenario(typeof(SadPathScenario), GetSadExecutionSteps(), "Sad Path Scenario", new List<string>()),
                new Scenario(typeof(SadPathScenario), GetInconclusiveExecutionSteps(), "Inconclusive Scenario", new List<string>()),
                new Scenario(typeof(SadPathScenario), GetNotImplementedExecutionSteps(), "Not Implemented Scenario", new List<string>())
            };

            // override specific step results - ideally this class could be refactored to provide  objectmother/builder interface
            SetAllStepResults(scenarios[0].Steps, Result.Passed);

            SetAllStepResults(scenarios[1].Steps, Result.Passed);
            scenarios[1].Steps.Last().Result = Result.Failed;
            scenarios[1].Steps.Last().Exception = new FakeExceptionWithStackTrace("This is a test exception.");

            return scenarios.ToArray();
        }

        private List<Step> GetHappyExecutionSteps()
        {
            var steps = new List<Step>
            {
                new Step(null, new StepTitle("Given a positive account balance"), true, ExecutionOrder.Assertion, true) {Duration = new TimeSpan(0, 0, 0, 0, 5), Result = Result.Passed},
                new Step(null, new StepTitle("When the account holder requests money"), true, ExecutionOrder.Assertion, true) {Duration = new TimeSpan(0, 0, 0, 0, 5), Result = Result.Passed},
                new Step(null, new StepTitle("Then money is dispensed"), true, ExecutionOrder.Assertion, true) {Duration = new TimeSpan(0, 0, 0, 0, 5), Result = Result.Passed},
            };
            return steps;
        }

        private List<Step> GetExampleExecutionSteps()
        {
            var steps = new List<Step>
            {
                new Step(null, new StepTitle("Given a <sign> account balance"), true, ExecutionOrder.Assertion, true) {Duration = new TimeSpan(0, 0, 0, 0, 5), Result = Result.Passed},
                new Step(null, new StepTitle("When the account holder requests money"), true, ExecutionOrder.Assertion, true) {Duration = new TimeSpan(0, 0, 0, 0, 5), Result = Result.Passed},
                new Step(null, new StepTitle("Then money <action> dispensed"), true, ExecutionOrder.Assertion, true) {Duration = new TimeSpan(0, 0, 0, 0, 5), Result = Result.Passed},
            };
            return steps;
        }

        private List<Step> GetSadExecutionSteps()
        {
            var steps = new List<Step>
            {
                new Step(null, new StepTitle("Given a negative account balance"), true, ExecutionOrder.Assertion, true) {Duration = new TimeSpan(0, 0, 0, 0, 5), Result = Result.Passed},
                new Step(null, new StepTitle("When the account holder requests money"), true, ExecutionOrder.Assertion, true) {Duration = new TimeSpan(0, 0, 0, 0, 5), Result = Result.Passed},
                new Step(null, new StepTitle("Then no money is dispensed"), true, ExecutionOrder.Assertion, true) {Duration = new TimeSpan(0, 0, 0, 0, 5), Result = Result.Passed},
            };
            return steps;
        }

        private List<Step> GetFailingExecutionSteps()
        {
            var steps = new List<Step>
            {
                new Step(null, new StepTitle("Given a negative account balance"), true, ExecutionOrder.Assertion, true),
                new Step(null, new StepTitle("When the account holder requests money"), true, ExecutionOrder.Assertion, true),
                new Step(null, new StepTitle("Then no money is dispensed"), true, ExecutionOrder.Assertion, true),
            };

            SetAllStepResults(steps, Result.Passed);

            var last = steps.Last();
            last.Result = Result.Failed;
            try
            {
                throw new InvalidOperationException("Boom");
            }
            catch (Exception ex)
            {
                last.Exception = ex;
            }

            return steps;
        }

        private List<Step> GetInconclusiveExecutionSteps()
        {
            var steps = new List<Step>
            {
                new Step(null, new StepTitle("Given a negative account balance"), true, ExecutionOrder.Assertion, true) {Duration = new TimeSpan(0, 0, 0, 0, 5)},
                new Step(null, new StepTitle("When the account holder requests money"), true, ExecutionOrder.Assertion, true) {Duration = new TimeSpan(0, 0, 0, 0, 5)},
                new Step(null, new StepTitle("Then no money is dispensed"), true, ExecutionOrder.Assertion, true) {Duration = new TimeSpan(0, 0, 0, 0, 5)},
            };

            SetAllStepResults(steps, Result.Passed);

            steps.Last().Result = Result.Inconclusive;

            return steps;
        }


        private List<Step> GetNotImplementedExecutionSteps()
        {
            var steps = new List<Step>
            {
                new Step(null, new StepTitle("Given a negative account balance"), true, ExecutionOrder.Assertion, true) {Duration = new TimeSpan(0, 0, 0, 0, 5)},
                new Step(null, new StepTitle("When the account holder requests money"), true, ExecutionOrder.Assertion, true) {Duration = new TimeSpan(0, 0, 0, 0, 5)},
                new Step(null, new StepTitle("Then no money is dispensed"), true, ExecutionOrder.Assertion, true) {Duration = new TimeSpan(0, 0, 0, 0, 5)},
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
        public class PlatinumAccountHolderStory { }
        public class ExampleScenario
        {
            public void GivenA__sign__AccountBalance() { }
            public void WhenTheAccountHolderRequestsMoney() { }
            public void ThenMoney__action__Dispensed() { }
        }
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

        class FakeExceptionWithStackTrace : Exception
        {
            public FakeExceptionWithStackTrace(string message)
                : base(message)
            { }

            public override string StackTrace
            {
                get { return "This is a test stack trace"; }
            }
        }
    }
}