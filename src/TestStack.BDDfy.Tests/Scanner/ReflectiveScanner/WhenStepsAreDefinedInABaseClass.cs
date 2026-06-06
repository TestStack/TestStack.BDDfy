using Shouldly;
using Xunit;

namespace TestStack.BDDfy.Tests.Scanner.ReflectiveScanner
{
    public class WhenStepsAreDefinedInABaseClass
    {
        private Story _story = null!;

        class TheBaseClass
        {
            public void GivenInTheBaseClass(){}
            public void WhenInTheBaseClass(){}
            public void ThenInTheBaseClass(){}
        }

        class TheSubClass : TheBaseClass
        {
            public void GivenInTheSubClass(){}
            public void WhenInTheSubClass(){}
            public void ThenInTheSubClass(){}
        }

        void Context()
        {
            _story = new TheSubClass().BDDfy();
        }

        Scenario Scenario
        {
            get
            {
                return _story.Scenarios.Single();
            }
        }

        [RunStepWithArgs("GivenInTheBaseClass", "Given in the base class", "And in the base class")]
        [RunStepWithArgs("WhenInTheBaseClass", "When in the base class", "And in the base class")]
        [RunStepWithArgs("ThenInTheBaseClass", "Then in the base class", "And in the base class")]
        void ThenTheFollowingStepFromBaseClassIsScanned(string stepName, string title, string promotedTitle)
        {
            Scenario.Steps.Any(s => s.Title == title || s.Title == promotedTitle).ShouldBe(true);
        }

        [RunStepWithArgs("GivenInTheSubClass", "Given in the sub class", "And in the sub class")]
        [RunStepWithArgs("WhenInTheSubClass", "When in the sub class", "And in the sub class")]
        [RunStepWithArgs("ThenInTheSubClass", "Then in the sub class", "And in the sub class")]
        void ThenTheFollowingStepFromSubClassScanned(string stepName, string title, string promotedTitle)
        {
            Scenario.Steps.Any(s => s.Title == title || s.Title == promotedTitle).ShouldBe(true);
        }

        [Fact]
        public void Execute()
        {
            this.BDDfy();
        }
    }
}