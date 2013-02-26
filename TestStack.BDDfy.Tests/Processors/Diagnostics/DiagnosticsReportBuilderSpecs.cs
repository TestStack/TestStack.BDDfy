using NSubstitute;
using NUnit.Framework;
using TestStack.BDDfy.Processors.Reporters;
using TestStack.BDDfy.Processors.Reporters.Diagnostics;
using TestStack.BDDfy.Processors.Reporters.Serializers;
using TestStack.BDDfy.Tests.Processors.Reports;

namespace TestStack.BDDfy.Tests.Processors.Diagnostics
{
    [TestFixture]
    public class DiagnosticsReportBuilderSpecs
    {
        [Test]
        public void ShouldSerializeDiagnosticDataToSpecifiedFormat()
        {
            var serializer = Substitute.For<ISerializer>();
            var testData = new ReportTestData().CreateTwoStoriesEachWithTwoScenariosWithThreeStepsOfFiveMilliseconds();
            var model = new FileReportModel(testData);
            var sut = new DiagnosticsReportBuilder(serializer);

            sut.CreateReport(model);
            
            serializer.Received().Serialize(Arg.Any<object>());
        }
    }
}