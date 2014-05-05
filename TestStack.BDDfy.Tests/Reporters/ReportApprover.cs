using System;
using System.Text.RegularExpressions;
using ApprovalTests;
using TestStack.BDDfy.Configuration;
using TestStack.BDDfy.Reporters;
using TestStack.BDDfy.Tests.Reporters.Html;

namespace TestStack.BDDfy.Tests.Reporters
{
    class ReportApprover
    {
        public static void Approve(Func<FileReportModel> model, IReportBuilder reportBuilder)
        {
            // somehow the scenario id keeps increasing on TC
            // resetting here explicitly
            Configurator.IdGenerator.Reset();

            // setting the culture to make sure the date is formatted the same on all machines
            using (new TemporaryCulture("en-GB"))
            {
                var result = reportBuilder.CreateReport(model());
                Approvals.Verify(result, s => Scrub(StackTraceScrubber.ScrubPaths(s)));
            }
        }

        static string Scrub(string scrubPaths)
        {
            return Regex.Replace(scrubPaths, @"(?<!\r)\n", "\r\n");
        }
    }
}
