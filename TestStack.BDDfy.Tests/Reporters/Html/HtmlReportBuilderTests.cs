using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using ApprovalTests;
using ApprovalTests.Reporters;
using NUnit.Framework;
using TestStack.BDDfy.Configuration;
using TestStack.BDDfy.Reporters.Html;

namespace TestStack.BDDfy.Tests.Reporters.Html
{
    [TestFixture]
    [UseReporter(typeof (DiffReporter))]
    public class HtmlReportBuilderTests
    {
        [Test]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void ShouldProduceExpectedHtml()
        {
            Configurator.IdGenerator.Reset();

            var model = new HtmlReportViewModel(
                new DefaultHtmlReportConfiguration(),
                new ReportTestData().CreateTwoStoriesEachWithTwoScenariosWithThreeStepsOfFiveMilliseconds());

            var sut = new HtmlReportBuilder {DateProvider = () => new DateTime(2014, 3, 25, 11, 30, 5)};

            // setting the culture to make sure the date is formatted the same on all machines
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
            
            // enforcing line ending explicitly
            var result = sut.CreateReport(model).Replace("\n", "\r\n");

            string expected = GetReportHtml();
            Assert.That(result, Is.EqualTo(expected));
            Approvals.Verify(result);
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