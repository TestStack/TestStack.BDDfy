using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using ApprovalTests;
using ApprovalTests.Reporters;
using NUnit.Framework;
using TestStack.BDDfy.Reporters.Html;

namespace TestStack.BDDfy.Tests.Reporters.Html
{
    [TestFixture]
    [UseReporter(typeof(DiffReporter))]
    public class HtmlReportBuilderTests
    {
        [Test]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void ShouldProduceExpectedHtml()
        {
            string expected = GetReportHtml(); 
            var model = new HtmlReportViewModel(
                new DefaultHtmlReportConfiguration(), 
                new ReportTestData().CreateTwoStoriesEachWithTwoScenariosWithThreeStepsOfFiveMilliseconds());
            var sut = new HtmlReportBuilder();
            sut.DateProvider = () => new DateTime(2014, 3, 25, 11, 30,5);

            var result = sut.CreateReport(model);

            //Approvals.Verify(result);
            Assert.That(result, Is.EqualTo(expected));
        }

        private string GetReportHtml()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "TestStack.BDDfy.Tests.Reporters.Html.HtmlReportBuilderTests.ShouldProduceExpectedHtml.approved.txt";

            string result;
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    result = reader.ReadToEnd();
                }
            }

            return result;
        }

    }
}