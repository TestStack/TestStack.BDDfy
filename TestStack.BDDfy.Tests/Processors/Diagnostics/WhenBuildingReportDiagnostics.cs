using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TestStack.BDDfy.Processors.Reporters;
using TestStack.BDDfy.Processors.Reporters.Diagnostics;
using TestStack.BDDfy.Tests.Processors.Reports;

namespace TestStack.BDDfy.Tests.Processors.Diagnostics
{
    [TestFixture]
    public class WhenBuildingReportDiagnostics
    {
        private DiagnosticsReportBuilder _sut;
        private IEnumerable<Core.Story> _stories; 
        private IList<StoryDiagnostic> _result;
            
        public void GivenADiagnosticsReportBuilder()
        {
            _sut = new DiagnosticsReportBuilder();
        }

        public void AndGivenTwoStoriesEachWithTwoScenariosWithThreeStepsOfFiveMilliseconds()
        {
            _stories = new ReportTestData().CreateTwoStoriesEachWithTwoScenariosWithThreeStepsOfFiveMilliseconds();
        }

        public void WhenTheDiagnosticDataIsCalculated()
        {
            _result = _sut.GetDiagnosticData(new FileReportModel(_stories));
        }

        public void ThenTwoStoriesShouldBeReturned()
        {
            Assert.AreEqual(2, _result.Count);
        }

        public void AndThenEachStoryShouldHaveDurationOf30Milliseconds()
        {
            _result.ToList().ForEach(story => Assert.AreEqual(30, story.Duration));
        }

        public void AndThenEachScenarioShouldHaveDurationOf10Milliseconds()
        {
            _result[0].Scenarios.ForEach(scenario => Assert.AreEqual(15, scenario.Duration));
            _result[1].Scenarios.ForEach(scenario => Assert.AreEqual(15, scenario.Duration));
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