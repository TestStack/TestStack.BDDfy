using System;
using System.Collections.Generic;
using System.Linq;
using Shouldly;
using TestStack.BDDfy.Configuration;
using Xunit;

namespace TestStack.BDDfy.Tests.Scanner.ReflectiveScanner
{
    public class WhenMethodNamesFollowNamingConventionsOtherThanGivenWhenThen
    {
        private List<Step> _steps;
        ScenarioClass _scenario;

        public WhenMethodNamesFollowNamingConventionsOtherThanGivenWhenThen()
        {
            var specEndMatcher = new MethodNameMatcher(s => s.EndsWith("specification", StringComparison.OrdinalIgnoreCase), false, ExecutionOrder.SetupState, true);
            var specStartMatcher = new MethodNameMatcher(s => s.StartsWith("specification", StringComparison.OrdinalIgnoreCase), false, ExecutionOrder.SetupState, true);
            var setupMethod = new MethodNameMatcher(s => s.Equals("Setup", StringComparison.OrdinalIgnoreCase), false, ExecutionOrder.SetupState, false);
            var assertMatcher = new MethodNameMatcher(s => s.StartsWith("Assert", StringComparison.Ordinal), true, ExecutionOrder.Assertion, true);
            var andAssertMatcher = new MethodNameMatcher(s => s.StartsWith("AndAssert", StringComparison.Ordinal), true, ExecutionOrder.ConsecutiveAssertion, true);
            var methodNameMatchers = new[] { assertMatcher, andAssertMatcher, specEndMatcher, specStartMatcher, setupMethod };
            _scenario = new ScenarioClass();
            var scanner = new MethodNameStepScanner(s => s, methodNameMatchers);
            _steps = scanner.Scan(TestContext.GetContext(_scenario)).ToList();
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

        [Fact]
        public void TheStepsAreFoundUsingConventionInjection()
        {
            _steps.Count.ShouldBe(5);
        }

        [Fact]
        public void TheSetupMethodIsPickedAsNonAsserting()
        {
            var setupMethod = _steps.Single(s => s.Title == "Setup");
            setupMethod.ExecutionOrder.ShouldBe(ExecutionOrder.SetupState);
            setupMethod.ShouldReport.ShouldBe(false);
            setupMethod.Asserts.ShouldBe(false);
        }

        [Fact]
        public void TheCorrectSpecificationStepsAreFound()
        {
            AssertSpecificationStepIsScannedProperly(_scenario.SpecificationAppearingInTheBeginningOfTheMethodName);
            AssertSpecificationStepIsScannedProperly(_scenario.AppearingAtTheEndOfTheMethodNameSpecification);
        }

        [Fact]
        public void IncorrectSpecificationStepIsNotAdded()
        {
            var specMethod = _steps.Where(s => s.Title == "This method specification should not be included");
            specMethod.ShouldBeEmpty();
        }

        void AssertSpecificationStepIsScannedProperly(Action getSpecMethod)
        {
            var specMethods = _steps.Where(s => s.Title.Trim() == Configurator.Scanners.Humanize(Helpers.GetMethodInfo(getSpecMethod).Name));
            specMethods.Count().ShouldBe(1);
            var specStep = specMethods.First();
            specStep.Asserts.ShouldBe(false);
            specStep.ShouldReport.ShouldBe(true);
            specStep.ExecutionOrder.ShouldBe(ExecutionOrder.SetupState);
        }
    }
}