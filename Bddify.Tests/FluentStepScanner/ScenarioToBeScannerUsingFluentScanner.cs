using Bddify.Core;
using Bddify.Scanners;
using Bddify.Scanners.GwtAttributes;

namespace Bddify.Tests.FluentStepScanner
{
    [Story]
    class ScenarioToBeScannerUsingFluentScanner
    {
        public void GivenSomeState(int input1, int input2)
        {
        }

        public void WhenSomeStateUsesIncompatibleNamingConvention()
        {
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

        public static IScanForSteps GetScanner()
        {
            return (IScanForSteps)new FluentStepScanner<ScenarioToBeScannerUsingFluentScanner>()
                .Given(s => s.GivenSomeState(1, 2))
                .And(s => s.WhenSomeStateUsesIncompatibleNamingConvention())
                .And(s => s.AndSomeStateWithIncorrectAttribute())
                .When(s => s.WhenSomethingHappens("some input here"))
                .And(s => s.AndThenSomethingElseHappens(), "Overriding step name without arguments")
                .And(s => s.WhenSomethingHappens("other input"), "step used with {0} for the second time")
                .Then(s => s.ThenTheFollowingAssertionsShouldBeCorrect())
                .And(s => s.AndIncorrectAttributeWouldNotMatter())
                .TearDownWith(s => s.Dispose());
        }
    }
}