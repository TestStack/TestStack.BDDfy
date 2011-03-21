using System.Collections.Generic;
using Bddify.Core;
using Bddify.Scanners;
using NUnit.Framework;
using System.Linq;

namespace Bddify.Tests.ScannerSpecs
{
    public class RunScenarioWithArgsContext
    {
        private List<Scenario> _result;

        [RunScenarioWithArgs(1, 2, 3)]
        [RunScenarioWithArgs(4, 5, 6)]
        private class ScenarioWithArgs
        {
        }

        [SetUp]
        public void Setup()
        {
            _result = new GwtScanner().Scan(new ScenarioWithArgs()).ToList();
        }

        [Test]
        public void ReturnsOneScenarioPerAttribute()
        {
            Assert.That(_result.Count, Is.EqualTo(2));
        }

        [Test]
        public void ArgsAreSetProperlyOnScenarios()
        {
            Assert.That(_result[0].ArgsSet, Is.EquivalentTo(new[]{1, 2, 3}));
            Assert.That(_result[1].ArgsSet, Is.EquivalentTo(new[]{4, 5, 6}));
        }
    }
}