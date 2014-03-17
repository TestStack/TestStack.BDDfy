using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.Linq;
using TestStack.BDDfy.Configuration;

namespace TestStack.BDDfy.Tests.Scanner
{
    [TestFixture]
    public class WhenMethodNamesFollowNamingConventionsOtherThanGivenWhenThen
    {
        private List<ExecutionStep> _steps;
        ScenarioClass _scenario;

        [SetUp]
        public void Setup()
        {
            var specEndMatcher = new MethodNameMatcher(s => s.EndsWith("specification", StringComparison.OrdinalIgnoreCase), false, ExecutionOrder.SetupState, true);
            var specStartMatcher = new MethodNameMatcher(s => s.StartsWith("specification", StringComparison.OrdinalIgnoreCase), false, ExecutionOrder.SetupState, true);
            var setupMethod = new MethodNameMatcher(s => s.Equals("Setup", StringComparison.OrdinalIgnoreCase), false, ExecutionOrder.SetupState, false);
            var assertMatcher = new MethodNameMatcher(s => s.StartsWith("Assert", StringComparison.Ordinal), true, ExecutionOrder.Assertion, true);
            var andAssertMatcher = new MethodNameMatcher(s => s.StartsWith("AndAssert", StringComparison.Ordinal), true, ExecutionOrder.ConsecutiveAssertion, true);
            var methodNameMatchers = new[] { assertMatcher, andAssertMatcher, specEndMatcher, specStartMatcher, setupMethod };
            _scenario = new ScenarioClass();
            var scanner = new MethodNameStepScanner(s => s, methodNameMatchers);
            _steps = scanner.Scan(_scenario).ToList();
        }

        class ScenarioClass
        {
            public void Setup()
            {
            }

            public void AndAssertThat()
            {
            }

            public void AssertThis()
            {
            }

            public void ThisMethodSpecificationShouldNotBeIncluded()
            {
            }

            public void SpecificationAppearingInTheBeginningOfTheMethodName()
            {
            }

            public void AppearingAtTheEndOfTheMethodNameSpecification()
            {
            }

            [IgnoreStep]
            public void SpecificationToIgnore()
            {
            }
        }

        [Test]
        public void TheStepsAreFoundUsingConventionInjection()
        {
            Assert.That(_steps.Count, Is.EqualTo(5));
        }

        [Test]
        public void TheSetupMethodIsPickedAsNonAsserting()
        {
            var setupMethod = _steps.Single(s => s.StepTitle == "Setup");
            Assert.That(setupMethod.ExecutionOrder, Is.EqualTo(ExecutionOrder.SetupState));
            Assert.That(setupMethod.ShouldReport, Is.False);
            Assert.That(setupMethod.Asserts, Is.False);
        }

        [Test]
        public void TheCorrectSpecificationStepsAreFound()
        {
            AssertSpecificationStepIsScannedProperly(_scenario.SpecificationAppearingInTheBeginningOfTheMethodName);
            AssertSpecificationStepIsScannedProperly(_scenario.AppearingAtTheEndOfTheMethodNameSpecification);
        }

        [Test]
        public void IncorrectSpecificationStepIsNotAdded()
        {
            var specMethod = _steps.Where(s => s.StepTitle == "This method specification should not be included");
            Assert.That(specMethod, Is.Empty);
        }

        void AssertSpecificationStepIsScannedProperly(Action getSpecMethod)
        {
            var specMethods = _steps.Where(s => s.StepTitle.Trim() == Configurator.Scanners.Humanize(Helpers.GetMethodInfo(getSpecMethod).Name));
            Assert.That(specMethods.Count(), Is.EqualTo(1));
            var specStep = specMethods.First();
            Assert.That(specStep.Asserts, Is.False);
            Assert.That(specStep.ShouldReport, Is.True);
            Assert.That(specStep.ExecutionOrder, Is.EqualTo(ExecutionOrder.SetupState));
        }
    }
}