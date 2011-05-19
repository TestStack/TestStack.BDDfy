// This shows how you can use bddify attributes to identify your test steps or ignore some of the methods that would have otherwise been identified and run as a step.
// Bddify can find your steps using naming conventions; but if you are not following any conventions then you may use executable attributes to 
// tell bddify which methods are steps and how it should run them.

using System;
using Bddify.Core;
using Bddify.Scanners;
using Bddify.Scanners.GwtAttributes;
using NUnit.Framework;

namespace $rootnamespace$.Bddify.Samples.MsTest
{
    public class ScenarioWithMixedSteps
    {
		// This is matched using naming convention; because the method name starts with 'When'
        void WhenThisClassIsBddified()
        { }

		// This is scanned as a 'And Then' step because of the 'AndThen' attribute
        [AndThen]
        void IncludingThisLegacyTestMethod()
        { }

		// This is matched using naming convention; because the method name starts with 'Given'
        void GivenThisScenarioHasAMixtureOfLegacyAndBddStyleTests()
        {}

		// This is scanned as a 'And When' step because of the 'AndWhen' attribute
        [AndWhen]
        void LegacyTransitionMethod()
        { }

		// This is matched using naming convention because the method name starts with 'Then'
        void ThenStepsAreScannedProperly()
        { }

		// This is scanned as a 'And Given' step because of the 'AndGiven' attribute
        [AndGiven]
        void AndThisIsLegacySetupMethod()
        {}

		// This method would have been identified as a 'And Then' step if it was not decoreated wit 'IgnoreStep'
		// If you have a method that matches naming conventions but you do not want bddify to run it as a step; 
		// you may decorate it with 'IgnoreStep' attribute
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