using System.Collections.Generic;
using Bddify.Core;
using Bddify.Scanners;
using NUnit.Framework;
using System.Linq;

namespace Bddify.Tests.Scanner
{
    public class WhenScenarioIsRunWithArguments
    {
        [RunScenarioWithArgs("Arg1", 2, 3)]
        [RunScenarioWithArgs("Arg2", 5, 6)]
        private class ScenarioWithArgs
        {
        }

        private static List<Scenario> ScanScenario(string scenarioTextTemplate = null)
        {
            return new ScanForScenarios(new[] {new DefaultScanForStepsByMethodName()}, scenarioTextTemplate).Scan(typeof(ScenarioWithArgs)).ToList();
        }

        [Test]
        public void ThenOneScenarioIsReturnedPerAttributeRegardlessOfTheTextTemplate()
        {
            var result = ScanScenario();
            Assert.That(result.Count, Is.EqualTo(2));

            result = ScanScenario("Some text for scenario");
            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public void ArgsAreSetProperlyOnScenarios()
        {
            var result = ScanScenario();
            var argsSets = result.Select(r => r.ArgsSet);
            Assert.That(argsSets.Count(a => a.Length != 3), Is.EqualTo(0));
            Assert.That(argsSets.Single(o => o[0].ToString() == "Arg1")[1], Is.EqualTo(2));
            Assert.That(argsSets.Single(o => o[0].ToString() == "Arg2")[2], Is.EqualTo(6));
        }

        [Test]
        public void ThenScenarioTextIsFetchedFromTheClassNameWhenTextTemplateIsNotProvided()
        {
            var result = ScanScenario();
            Assert.True(result.Single(s => s.ScenarioText == "Scenario with args Arg1, 2, 3").ArgsSet.SequenceEqual(new object[] { "Arg1", 2, 3 }));
            Assert.True(result.Single(s => s.ScenarioText == "Scenario with args Arg2, 5, 6").ArgsSet.SequenceEqual(new object[] { "Arg2", 5, 6 }));
        }

        [Test]
        public void ThenScenarioTextIsSetBasedOnTheTextTemplateWhenTextTemplateIsProvided()
        {
            var result = ScanScenario("Some template arg1:{0}, arg2:{1}, arg3:{2}");
            Assert.True(result.Single(s => s.ScenarioText == "Some template arg1:Arg1, arg2:2, arg3:3").ArgsSet.SequenceEqual(new object[] { "Arg1", 2, 3 }));
            Assert.True(result.Single(s => s.ScenarioText == "Some template arg1:Arg2, arg2:5, arg3:6").ArgsSet.SequenceEqual(new object[] { "Arg2", 5, 6 }));
        }
    }
}