using System;
using Bddify.Scanners;
using Bddify.Scanners.GwtAttributes;
using NUnit.Framework;

namespace Demos.NUnit
{
    public class ScenarioWithMixedSteps
    {
        void GivenThisScenarioHasAMixtureOfLegacyAndBddStyleTests()
        {}

        [AndGiven]
        void AndThisIsLegacySetupMethod()
        {}

        void WhenThisClassIsBddified()
        {}

        [AndWhen]
        void LegacyTransitionMethod()
        {}

        void ThenStepsAreScannedProperly()
        {}

        [AndThen]
        void IncludingThisLegacyTestMethod()
        {}

        [IgnoreStep]
        void AndThisMethodIsNotReturned()
        {
            throw new Exception("This method should not be returned");
        }

        [Test]
        public void Execute()
        {
            this.Bddify();
        }
    }
}