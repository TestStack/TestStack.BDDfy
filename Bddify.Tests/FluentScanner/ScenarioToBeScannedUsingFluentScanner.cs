using System.Linq;
using Bddify.Core;
using Bddify.Scanners.StepScanners;
using Bddify.Scanners.StepScanners.ExecutableAttribute.GwtAttributes;
using Bddify.Scanners.StepScanners.Fluent;
using System.Collections.Generic;

namespace Bddify.Tests.FluentScanner
{
    [Story]
    class ScenarioToBeScannedUsingFluentScanner
    {
        private string[] _input1;
        private int[] _input2;
        private int _input3;

        public int Input3
        {
            get { return _input3; }
        }

        public int[] Input2
        {
            get { return _input2; }
        }

        public string[] Input1
        {
            get { return _input1; }
        }

        public void GivenSomeState(int input1, int input2)
        {
        }

        public void WhenSomeStepUsesIncompatibleNamingConvention()
        {
        }

        public void AndAMethodTakesArrayInputs(string[] input1, int[] input2, int input3)
        {
            _input1 = input1;
            _input2 = input2;
            _input3 = input3;
        }

        [Then]
        public void AndSomeStateWithIncorrectAttribute()
        { }

        public void WhenSomethingHappens(string input1)
        {
        }

        public void AndThenSomethingElseHappens()
        {
        }

        public void ThenTheFollowingAssertionsShouldBeCorrect()
        { }

        [When]
        public void AndIncorrectAttributeWouldNotMatter()
        { }

        public void Dispose()
        {}

        public static IEnumerable<ExecutionStep> GetSteps(ScenarioToBeScannedUsingFluentScanner testObject)
        {
            var fluentScanner = testObject
                .Given(s => s.GivenSomeState(1, 2))
                    .And(s => s.WhenSomeStepUsesIncompatibleNamingConvention())
                    .And(s => s.AndAMethodTakesArrayInputs(new[] {"1", "2"}, new[] {3, 4}, 5))
                    .And(s => s.AndSomeStateWithIncorrectAttribute())
                .When(s => s.WhenSomethingHappens("some input here"))
                    .And(s => s.AndThenSomethingElseHappens(), "Overriding step name without arguments")
                    .And(s => s.WhenSomethingHappens("other input"), "step used with {0} for the second time")
                .Then(s => s.ThenTheFollowingAssertionsShouldBeCorrect())
                    .And(s => s.AndIncorrectAttributeWouldNotMatter())
                .TearDownWith(s => s.Dispose());

            return fluentScanner.GetScanner(null).Scan().Scenarios.SelectMany(s => s.Steps).ToList();
        }
    }
}