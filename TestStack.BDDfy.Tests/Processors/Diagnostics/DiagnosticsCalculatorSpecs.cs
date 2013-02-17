using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TestStack.BDDfy.Processors;
using TestStack.BDDfy.Processors.Diagnostics;

namespace TestStack.BDDfy.Tests.Processors.Diagnostics
{
    [TestFixture]
    public class DiagnosticsCalculatorSpecs
    {
        private DiagnosticsCalculator _sut;
        private IEnumerable<Core.Story> _stories; 
        private IList<StoryDiagnostic> _result;
            
        public void GivenADiagnosticsCalculator()
        {
            _sut = new DiagnosticsCalculator();
        }

        public void AndGivenTwoStoriesEachWithTwoScenariosWithTwoStepsOfFiveMilliseconds()
        {
            _stories = new DiagnosticTestData().CreateTwoStoriesEachWithTwoScenariosWithTwoStepsOfFiveMilliseconds();
        }

        public void WhenTheDiagnosticDataIsCalculated()
        {
            _result = _sut.GetDiagnosticData(new FileReportModel(_stories));
        }

        public void ThenTwoStoriesShouldBeReturned()
        {
            Assert.AreEqual(2, _result.Count);
        }

        public void AndThenEachStoryShouldHaveDurationOf20Milliseconds()
        {
            _result.ToList().ForEach(story => Assert.AreEqual(20, story.Duration));
        }

        public void AndThenEachScenarioShouldHaveDurationOf10Milliseconds()
        {
            _result[0].Scenarios.ForEach(scenario => Assert.AreEqual(10, scenario.Duration));
            _result[1].Scenarios.ForEach(scenario => Assert.AreEqual(10, scenario.Duration));
        }

        public void AndThenEachStepShouldHaveDurationOf5Milliseconds()
        {
            _result[0].Scenarios.ForEach(scenario => scenario.Steps.ForEach(step => Assert.AreEqual(5, step.Duration)));
            _result[1].Scenarios.ForEach(scenario => scenario.Steps.ForEach(step => Assert.AreEqual(5, step.Duration)));
        }

        [Test]
        public void RunSpecs()
        {
            this.BDDfy();
        }
    }
}