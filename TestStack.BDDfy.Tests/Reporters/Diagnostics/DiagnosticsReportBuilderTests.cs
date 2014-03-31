using NSubstitute;
using NUnit.Framework;
using TestStack.BDDfy.Reporters;
using TestStack.BDDfy.Reporters.Diagnostics;
using TestStack.BDDfy.Reporters.Serializers;

namespace TestStack.BDDfy.Tests.Reporters.Diagnostics
{
    [TestFixture]
    public class DiagnosticsReportBuilderTests
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