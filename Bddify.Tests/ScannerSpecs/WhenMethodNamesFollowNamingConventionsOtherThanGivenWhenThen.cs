using System;
using System.Collections.Generic;
using Bddify.Core;
using Bddify.Scanners;
using NUnit.Framework;
using System.Linq;

namespace Bddify.Tests.ScannerSpecs
{
    public class WhenMethodNamesFollowNamingConventionsOtherThanGivenWhenThen
    {
        private List<ExecutionStep> _steps;
        private const string SetupMethodName = "Setup";

        [SetUp]
        public void Setup()
        {
            var specEndMatcher = new MethodNameMatcher(s => s.EndsWith("specification", StringComparison.OrdinalIgnoreCase), true);
            var specStartMatcher = new MethodNameMatcher(s => s.StartsWith("specification", StringComparison.OrdinalIgnoreCase), true);
            var setupMethod = new MethodNameMatcher(s => s.Equals(SetupMethodName, StringComparison.OrdinalIgnoreCase), false, false);
            var methodNameMatchers = new[] { specEndMatcher, specStartMatcher, setupMethod };
            var scanner = new ScanForStepsByMethodName(methodNameMatchers);
            _steps = scanner.Scan(this.GetType()).ToList();
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

        [Test]
        public void TheStepsAreFoundUsingConventionInjection()
        {
            Assert.That(_steps.Count, Is.EqualTo(3));
        }

        [Test]
        public void TheSetupMethodIsPickedAsNonAsserting()
        {
            var setupMethod = _steps.Single(s => s.ReadableMethodName == SetupMethodName);
            Assert.That(setupMethod.ShouldReport, Is.False);
            Assert.That(setupMethod.Asserts, Is.False);
        }

        [Test]
        public void TheSpecificationMethodsAreAssertingAndReporting()
        {
            var specMethods = _steps.Where(s => !s.ReadableMethodName.Contains(SetupMethodName));
            Assert.That(specMethods.Count(), Is.EqualTo(2));
            Assert.That(specMethods.Count(s => s.Asserts), Is.EqualTo(2));
            Assert.That(specMethods.Count(s => s.ShouldReport), Is.EqualTo(2));
        }
    }
}