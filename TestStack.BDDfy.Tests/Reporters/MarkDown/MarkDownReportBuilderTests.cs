using System.Runtime.CompilerServices;
using ApprovalTests;
using NUnit.Framework;
using TestStack.BDDfy.Configuration;
using TestStack.BDDfy.Reporters;
using TestStack.BDDfy.Reporters.MarkDown;
using TestStack.BDDfy.Tests.Reporters.Html;

namespace TestStack.BDDfy.Tests.Reporters.MarkDown
{
    [TestFixture]
    public class MarkDownReportBuilderTests
    {
        [Test]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void ShouldProduceExpectedMarkdown()
        {
            // somehow the scenario id keeps increasing on TC
            // resetting here explicitly
            Configurator.IdGenerator.Reset();

            // setting the culture to make sure the date is formatted the same on all machines
            using (new TemporaryCulture("en-GB"))
            {
                var model = new FileReportModel(new ReportTestData().CreateMixContainingEachTypeOfOutcome());

                var sut = new MarkDownReportBuilder();
                var result = sut.CreateReport(model);
                Approvals.Verify(result);
            }
        }
    }
}
