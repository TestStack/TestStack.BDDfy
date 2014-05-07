using System;
using System.Runtime.CompilerServices;
using ApprovalTests;
using NUnit.Framework;
using TestStack.BDDfy.Reporters;
using TestStack.BDDfy.Reporters.Html;
using TestStack.BDDfy.Reporters.MarkDown;
using TestStack.BDDfy.Tests.Reporters;

namespace TestStack.BDDfy.Tests
{
    [TestFixture]
    public class TagsTests
    {
        [Test]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void TagsAreReportedInTextReport()
        {
            var story = this.Given(_ => GivenAStep())
                .WithTags("Tag1", "Tag 2")
                .BDDfy();

            var textReporter = new TextReporter();
            textReporter.Process(story);

            Approvals.Verify(textReporter.ToString());
        }

        [Test]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void TagsAreReportedInHtmlReport()
        {
            var story = this.Given(_ => GivenAStep())
                .WithTags("Tag1", "Tag 2")
                .BDDfy();
            Func<FileReportModel> model = () => new HtmlReportModel(new[] { story })
                {
                    RunDate = new DateTime(2014, 3, 25, 11, 30, 5)
                };

            var sut = new ClassicReportBuilder();
            ReportApprover.Approve(model, sut);
        }

        [Test]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void TagsAreReportedInMetroHtmlReport()
        {
            var story = this.Given(_ => GivenAStep())
                .WithTags("Tag1", "Tag 2")
                .BDDfy();
            Func<FileReportModel> model = () => new HtmlReportModel(new[] { story })
                {
                    RunDate = new DateTime(2014, 3, 25, 11, 30, 5)
                };

            var sut = new MetroReportBuilder();
            ReportApprover.Approve(model, sut);
        }

        [Test]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void TagsAreReportedInMarkdownReport()
        {
            var story = this.Given(_ => GivenAStep())
                .WithTags("Tag1", "Tag 2")
                .BDDfy();
            Func<FileReportModel> model = () => new FileReportModel(new[] { story });
            var sut = new MarkDownReportBuilder();
            ReportApprover.Approve(model, sut);
        }

        private void GivenAStep()
        {

        }
    }
}