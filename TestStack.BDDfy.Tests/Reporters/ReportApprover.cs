using System;
using System.Text.RegularExpressions;
using ApprovalTests;
using ApprovalTests.Utilities;
using TestStack.BDDfy.Reporters;
using TestStack.BDDfy.Tests.Reporters.Html;

namespace TestStack.BDDfy.Tests.Reporters
{
    class ReportApprover
    {
        public static void Approve(FileReportModel model, IReportBuilder reportBuilder)
        {
            // setting the culture to make sure the date is formatted the same on all machines
            using (new TemporaryCulture("en-GB"))
            {
                var result = reportBuilder.CreateReport(model);
                Approvals.Verify(result, s => Scrub(StackTraceScrubber.ScrubLineNumbers(StackTraceScrubber.ScrubPaths(s))));
            }
        }

        static string Scrub(string scrubPaths)
        {
            return Regex.Replace(Regex.Replace(Regex.Replace(scrubPaths, @"(?<!\r)\n", "\r\n"), "step-\\d+-\\d+", "step-1-1"), "scenario-\\d+", "scenario-1");
        }
    }
}
