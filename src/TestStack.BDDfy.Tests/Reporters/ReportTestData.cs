namespace TestStack.BDDfy.Tests.Reporters
{
    using System.Linq;

    public class ReportTestData
    {
        private static readonly TimeSpan StepDuration = TimeSpan.FromMilliseconds(5);

        private static readonly StoryMetadata HappinessStory = new(
            typeof(RegularAccountHolderStory),
            "As a person", "I want ice cream", "So that I can be happy", "Happiness");

        private static readonly StoryMetadata AccountHolderStory = new(
            typeof(GoldAccountHolderStory),
            "As an account holder", "I want to withdraw cash",
            "So that I can get money when the bank is closed", "Account holder withdraws cash");

        private int _idCount;

        public IEnumerable<Story> CreateTwoStoriesEachWithOneFailingScenarioAndOnePassingScenarioWithThreeStepsOfFiveMilliseconds(bool? includeExamples = false)
        {
            return
            [
                new(HappinessStory, GetScenarios(includeFailingScenario: false, includeExamples: includeExamples ?? false)),
                new(AccountHolderStory, GetScenarios(includeFailingScenario: true, includeExamples: includeExamples ?? false))
            ];
        }

        public IEnumerable<Story> CreateMixContainingEachTypeOfOutcome()
        {
            return
            [
                new(HappinessStory, GetOneOfEachScenarioResult()),
                new(AccountHolderStory, GetOneOfEachScenarioResult()),
                new(null!, GetOneOfEachScenarioResult())
            ];
        }

        public IEnumerable<Story> CreateMixContainingEachTypeOfOutcomeWithOneScenarioPerStory()
        {
            var unhappyExamplesStory = new StoryMetadata(
                typeof(GoldAccountHolderStory),
                "As an unhappy examples story", "I want to see failed steps",
                "So that I can diagnose what's wrong", "Unhappy examples");

            var happyExamplesStory = new StoryMetadata(
                typeof(PlatinumAccountHolderStory),
                "As a happy examples story", "I want a clean report with examples",
                "So that the report is clean and readable", "Happy Examples");

            return
            [
                new(HappinessStory, CreateScenario<HappyPathScenario>("Happy Path Scenario [for Happiness]", Result.Passed)),
                new(HappinessStory, CreateScenario<SadPathScenario>("Sad Path Scenario [for Happiness]", Result.Failed)),
                new(HappinessStory, CreateScenario<SadPathScenario>("Inconclusive Scenario [for Happiness]", Result.Inconclusive)),
                new(HappinessStory, CreateScenario<SadPathScenario>("Not Implemented Scenario [for Happiness]", Result.NotImplemented)),
                new(null!, CreateScenario<HappyPathScenario>("Happy Path Scenario [with no story]", Result.Passed)),
                new(null!, CreateScenario<SadPathScenario>("Sad Path Scenario [with no story]", Result.Failed)),
                new(null!, CreateScenario<SadPathScenario>("Inconclusive Scenario [with no story]", Result.Inconclusive)),
                new(null!, CreateScenario<SadPathScenario>("Not Implemented Scenario [with no story]", Result.NotImplemented)),
                new(unhappyExamplesStory, GetScenarios(includeFailingScenario: true, includeExamples: true)),
                new(happyExamplesStory, GetScenarios(includeFailingScenario: false, includeExamples: true)),
            ];
        }

        public IEnumerable<Story> CreateTwoStoriesEachWithOneFailingScenarioAndOnePassingScenarioWithThreeStepsOfFiveMillisecondsAndEachHasTwoExamples()
        {
            return
            [
                new(HappinessStory, GetScenarios(includeFailingScenario: false, includeExamples: true)),
                new(AccountHolderStory, GetScenarios(includeFailingScenario: true, includeExamples: true))
            ];
        }

        private Scenario[] GetScenarios(bool includeFailingScenario, bool includeExamples)
        {
            if (includeExamples)
                return GetExampleScenarios(includeFailingScenario);

            var sadSteps = CreateSadSteps(includeFailingScenario ? Result.Failed : Result.Passed);
            return
            [
                new(typeof(HappyPathScenario), CreateHappySteps(), "Happy Path Scenario", []),
                new(typeof(SadPathScenario), sadSteps, "Sad Path Scenario", [])
            ];
        }

        private Scenario[] GetExampleScenarios(bool includeFailingScenario)
        {
            var exampleId = _idCount++.ToString();
            var exampleTable = new ExampleTable("sign", "action")
            {
                { "positive", "is" },
                { "negative", "is not" }
            };

            var lastStepResult = includeFailingScenario ? Result.Failed : Result.Passed;
            var exceptionMessage = includeFailingScenario ? "Boom\nWith\r\nNew lines" : null;

            return
            [
                new(exampleId, typeof(ExampleScenario), CreateExampleSteps(), "Example Scenario", exampleTable.ElementAt(0), []),
                new(exampleId, typeof(ExampleScenario), CreateExampleSteps(lastStepResult, exceptionMessage), "Example Scenario", exampleTable.ElementAt(1), [])
            ];
        }

        private Scenario[] GetOneOfEachScenarioResult()
        {
            return
            [
                new(typeof(HappyPathScenario), CreateHappySteps(), "Happy Path Scenario", []),
                new(typeof(SadPathScenario), CreateSadSteps(Result.Failed), "Sad Path Scenario", []),
                new(typeof(SadPathScenario), CreateSadSteps(Result.Inconclusive), "Inconclusive Scenario", []),
                new(typeof(SadPathScenario), CreateSadSteps(Result.NotImplemented), "Not Implemented Scenario", [])
            ];
        }

        private static Scenario CreateScenario<TScenario>(string title, Result lastStepResult)
        {
            var steps = lastStepResult == Result.Passed
                ? CreateHappySteps()
                : CreateSadSteps(lastStepResult);
            return new(typeof(TScenario), steps, title, []);
        }

        private static List<Step> CreateHappySteps() => CreateSteps(
            ["Given a positive account balance", "When the account holder requests money", "Then money is dispensed"],
            Result.Passed);

        private static List<Step> CreateSadSteps(Result lastStepResult) => CreateSteps(
            ["Given a negative account balance", "When the account holder requests money", "Then no money is dispensed"],
            lastStepResult,
            lastStepResult == Result.Failed ? "Boom" : null);

        private static List<Step> CreateExampleSteps(Result lastStepResult = Result.Passed, string? exceptionMessage = null) => CreateSteps(
            ["Given a <sign> account balance", "When the account holder requests money", "Then money <action> dispensed"],
            lastStepResult,
            exceptionMessage);

        private static List<Step> CreateSteps(string[] titles, Result lastStepResult, string? exceptionMessage = null)
        {
            var steps = titles.Select(title =>
                new Step(null, new StepTitle(title), true, ExecutionOrder.Assertion, true, [])
                {
                    Duration = StepDuration,
                    Result = Result.Passed
                }).ToList();

            var last = steps.Last();
            last.Result = lastStepResult;

            if (lastStepResult == Result.Failed)
                last.Exception = CaptureException(exceptionMessage ?? "Boom");

            return steps;
        }

        private static Exception CaptureException(string message)
        {
            try { throw new InvalidOperationException(message); }
            catch (Exception ex) { return ex; }
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
    }
}
