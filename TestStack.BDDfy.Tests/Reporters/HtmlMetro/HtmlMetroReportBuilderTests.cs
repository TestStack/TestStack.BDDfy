using System;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using TestStack.BDDfy.Reporters;
using TestStack.BDDfy.Reporters.Html;
using TestStack.BDDfy.Reporters.HtmlMetro;

namespace TestStack.BDDfy.Tests.Reporters.HtmlMetro
{
    [TestFixture]
    public class HtmlMetroReportBuilderTests
    {
        [Test]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void ShouldProduceExpectedHtml()
        {
            Func<FileReportModel> model = () => 
                new HtmlReportModel(new ReportTestData().CreateMixContainingEachTypeOfOutcome())
                    {
                        RunDate = new DateTime(2014, 3, 25, 11, 30, 5)
                    };

            var sut = new HtmlMetroReportBuilder();
            ReportApprover.Approve(model, sut);
        }
    }
}