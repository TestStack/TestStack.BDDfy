using Bddify.Core;
using NUnit.Framework;
using System.Linq;

namespace Bddify.Tests.Scanner
{
    public class WhenStepsAreDefinedInABaseClass
    {
        private Core.Story _story;

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
            var scenario = new TheSubClass();
            var bddifier = scenario.LazyBddify();
            bddifier.Run();
            _story = bddifier.Story;
        }

        TheSubClass Subject
        {
            get
            {
                return (TheSubClass)Scenario.TestObject;
            }
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
            Assert.That(Scenario.Steps.Count(s=> s.ReadableMethodName == NetToString.Convert(stepName)), Is.EqualTo(1));
        }

        [RunStepWithArgs("GivenInTheSubClass")]
        [RunStepWithArgs("WhenInTheSubClass")]
        [RunStepWithArgs("ThenInTheSubClass")]
        void ThenTheFollowingStepFromSubClassScanned(string stepName)
        {
            Assert.That(Scenario.Steps.Count(s => s.ReadableMethodName == NetToString.Convert(stepName)), Is.EqualTo(1));
        }

        [Test]
        public void Execute()
        {
            this.Bddify();
        }
    }
}