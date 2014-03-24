using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using TestStack.BDDfy.Reporters.Html;

namespace TestStack.BDDfy.Tests.Reporters.Html
{
    [TestFixture]
    public class HtmlReportBuilderSpecs
    {
        [Test]
        public void ShouldProduceExpectedHtml()
        {
            string expected = GetReportHtml();
            var model = new HtmlReportViewModel(
                new DefaultHtmlReportConfiguration(), 
                new ReportTestData().CreateTwoStoriesEachWithTwoScenariosWithThreeStepsOfFiveMilliseconds());
            var sut = new HtmlReportBuilder();

            var result = sut.CreateReport(model);

            // would prefer to assert the string contents but different IDs are generated each time so reports are never exacty the same.
            Assert.That(result.Length, Is.EqualTo(expected.Length));
        }

        private string GetReportHtml()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "TestStack.BDDfy.Tests.Reporters.Html.HtmlReport.approved.html";

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