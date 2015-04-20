using System.Collections.Generic;
using System.Linq;
using Shouldly;
using TestStack.BDDfy.Reporters;
using TestStack.BDDfy.Reporters.Diagnostics;
using Xunit;

namespace TestStack.BDDfy.Tests.Reporters.Diagnostics
{
    public class WhenBuildingReportDiagnostics
    {
        private DiagnosticsReportBuilder _sut;
        private IEnumerable<Story> _stories; 
        private IList<StoryDiagnostic> _result;
            
        public void GivenADiagnosticsReportBuilder()
        {
            _sut = new DiagnosticsReportBuilder();
        }

        public void AndGivenTwoStoriesEachWithTwoScenariosWithThreeStepsOfFiveMilliseconds()
        {
            _stories = new ReportTestData().CreateTwoStoriesEachWithOneFailingScenarioAndOnePassingScenarioWithThreeStepsOfFiveMilliseconds();
        }

        public void WhenTheDiagnosticDataIsCalculated()
        {
            _result = _sut.GetDiagnosticData(new FileReportModel(_stories.ToReportModel()));
        }

        public void ThenTwoStoriesShouldBeReturned()
        {
            _result.Count.ShouldBe(2);
        }

        public void AndThenEachStoryShouldHaveDurationOf30Milliseconds()
        {
            _result.ToList().ForEach(story => story.Duration.ShouldBe(30));
        }

        public void AndThenEachScenarioShouldHaveDurationOf10Milliseconds()
        {
            _result[0].Scenarios.ForEach(scenario => scenario.Duration.ShouldBe(15));
            _result[1].Scenarios.ForEach(scenario => scenario.Duration.ShouldBe(15));
        }

        public void AndThenEachStepShouldHaveDurationOf5Milliseconds()
        {
            _result[0].Scenarios.ForEach(scenario => scenario.Steps.ForEach(step => step.Duration.ShouldBe(5)));
            _result[1].Scenarios.ForEach(scenario => scenario.Steps.ForEach(step => step.Duration.ShouldBe(5)));
        }

        [Fact]
        public void RunSpecs()
        {
            this.BDDfy();
        }
    }
}