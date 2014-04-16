using System;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using TestStack.BDDfy.Reporters;
using TestStack.BDDfy.Reporters.MarkDown;

namespace TestStack.BDDfy.Tests.Reporters.MarkDown
{
    [TestFixture]
    public class MarkDownReportBuilderTests
    {
        [Test]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void ShouldProduceExpectedMarkdown()
        {
            Func<FileReportModel> model = () => new FileReportModel(new ReportTestData().CreateMixContainingEachTypeOfOutcome());
            var sut = new MarkDownReportBuilder();
            ReportApprover.Approve(model, sut);
        }
    }
}
