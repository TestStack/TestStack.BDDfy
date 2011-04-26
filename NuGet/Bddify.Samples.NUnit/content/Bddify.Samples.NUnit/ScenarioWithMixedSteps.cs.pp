using System;
using Bddify.Core;
using Bddify.Scanners;
using Bddify.Scanners.GwtAttributes;
using NUnit.Framework;

namespace $rootnamespace$.Bddify.Samples.NUnit
{
    public class ScenarioWithMixedSteps
    {
        void WhenThisClassIsBddified()
        { }

        [AndThen]
        void IncludingThisLegacyTestMethod()
        { }

        void GivenThisScenarioHasAMixtureOfLegacyAndBddStyleTests()
        {}

        [AndWhen]
        void LegacyTransitionMethod()
        { }

        void ThenStepsAreScannedProperly()
        { }

        [AndGiven]
        void AndThisIsLegacySetupMethod()
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