using System.Collections.Generic;
using Bddify.Core;
using Bddify.Scanners;
using NUnit.Framework;
using System.Linq;

namespace Bddify.Tests.Scanner
{
    public class WhenScenarioIsRunWithArguments
    {
        private List<Scenario> _result;

        [RunScenarioWithArgs("Arg1", 2, 3)]
        [RunScenarioWithArgs("Arg2", 5, 6)]
        private class ScenarioWithArgs
        {
        }

        [SetUp]
        public void Setup()
        {
            _result = new ScanForScenarios(new DefaultScanForStepsByMethodName()).Scan(typeof(ScenarioWithArgs)).ToList();
        }

        [Test]
        public void ThenOneScenarioIsReturnedPerAttribute()
        {
            Assert.That(_result.Count, Is.EqualTo(2));
        }

        [Test]
        public void ArgsAreSetProperlyOnScenarios()
        {
            var argsSets = _result.Select(r => r.ArgsSet);
            Assert.That(argsSets.Count(a => a.Length != 3), Is.EqualTo(0));
            Assert.That(argsSets.Single(o => o[0].ToString() == "Arg1")[1], Is.EqualTo(2));
            Assert.That(argsSets.Single(o => o[0].ToString() == "Arg2")[2], Is.EqualTo(6));
        }
    }
}