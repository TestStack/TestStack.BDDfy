﻿using System;
using System.Runtime.CompilerServices;
using ApprovalTests;
using TestStack.BDDfy.Reporters;
using TestStack.BDDfy.Reporters.Html;
using TestStack.BDDfy.Reporters.MarkDown;
using TestStack.BDDfy.Tests.Reporters;
using Xunit;

namespace TestStack.BDDfy.Tests
{
    public class TagsTests
    {
        [Fact]
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

        [Fact]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void TagsAreReportedInHtmlReport()
        {
            var model = new HtmlReportModel(this.CreateReportModel())            {
                RunDate = new DateTime(2014, 3, 25, 11, 30, 5)
            };

            var sut = new ClassicReportBuilder();
            ReportApprover.Approve(model, sut);
        }

        [Fact]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void TagsAreReportedInMetroHtmlReport()
        {
            var reportModel = this.CreateReportModel();
            var model = new HtmlReportModel(reportModel)
            {
                RunDate = new DateTime(2014, 3, 25, 11, 30, 5)
            };

            var sut = new MetroReportBuilder();
            ReportApprover.Approve(model, sut);
        }

        [Fact]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void TagsAreReportedInMarkdownReport()
        {
            var reportModel = this.CreateReportModel();
            var model = new FileReportModel(reportModel);
            var sut = new MarkDownReportBuilder();
            ReportApprover.Approve(model, sut);
        }

        private void GivenAStep()
        {

        }

        private ReportModel CreateReportModel()
        {
            var story = this.Given(_ => GivenAStep())
                .WithTags("Tag1", "Tag 2")
                .BDDfy();
            var reportModel = new[] { story }.ToReportModel();
            return reportModel;
        }
    }
}