using NSubstitute;
using Shouldly;
using TestStack.BDDfy.Reporters;
using TestStack.BDDfy.Reporters.Diagnostics;
using TestStack.BDDfy.Reporters.Serializers;
using Xunit;

namespace TestStack.BDDfy.Tests.Reporters.Diagnostics
{
    public class DiagnosticsReportBuilderTests
    {
        [Fact]
        public void ShouldSerializeDiagnosticDataToSpecifiedFormat()
        {
            var serializer = Substitute.For<ISerializer>();
            var testData = new ReportTestData().CreateTwoStoriesEachWithOneFailingScenarioAndOnePassingScenarioWithThreeStepsOfFiveMilliseconds();
            var model = new FileReportModel(testData.ToReportModel());
            var sut = new DiagnosticsReportBuilder(new CustomJsonSerializer());

            sut.CreateReport(model).ShouldMatchApproved();
        }
    }
}