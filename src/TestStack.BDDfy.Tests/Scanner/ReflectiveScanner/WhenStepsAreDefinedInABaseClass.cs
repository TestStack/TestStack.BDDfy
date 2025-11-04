using System.Linq;
using Shouldly;
using TestStack.BDDfy.Configuration;
using Xunit;

namespace TestStack.BDDfy.Tests.Scanner.ReflectiveScanner
{
    public class WhenStepsAreDefinedInABaseClass
    {
        private Story _story;

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

        [RunStepWithArgs("GivenInTheBaseClass")]
        [RunStepWithArgs("WhenInTheBaseClass")]
        [RunStepWithArgs("ThenInTheBaseClass")]
        void ThenTheFollowingStepFromBaseClassIsScanned(string stepName)
        {
            Scenario.Steps.Count(s => s.Title == Configurator.Humanizer.Humanize(stepName)).ShouldBe(1);
        }

        [RunStepWithArgs("GivenInTheSubClass")]
        [RunStepWithArgs("WhenInTheSubClass")]
        [RunStepWithArgs("ThenInTheSubClass")]
        void ThenTheFollowingStepFromSubClassScanned(string stepName)
        {
            Scenario.Steps.Count(s => s.Title == Configurator.Humanizer.Humanize(stepName)).ShouldBe(1);
        }

        [Fact]
        public void Execute()
        {
            this.BDDfy();
        }
    }
}