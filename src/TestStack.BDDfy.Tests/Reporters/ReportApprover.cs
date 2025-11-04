using System.Text.RegularExpressions;
using Shouldly;
using TestStack.BDDfy.Reporters;
using TestStack.BDDfy.Tests.Reporters.Html;

namespace TestStack.BDDfy.Tests.Reporters
{
    internal class ReportApprover
    {
        public static void Approve(FileReportModel model, IReportBuilder reportBuilder)
        {
            // setting the culture to make sure the date is formatted the same on all machines
            using (new TemporaryCulture("en-GB"))
            {
                var result = reportBuilder.CreateReport(model);
                result.ShouldMatchApproved(c => c
                    .WithScrubber(Scrub)
                    .UseCallerLocation());
            }
        }

        public static string Scrub(string scrubPaths)
        {
            return ScrubLineNumbers(ScrubPaths(
                Regex.Replace(
                    Regex.Replace(Regex.Replace(scrubPaths, @"(?<!\r)\n", "\r\n"), "step-\\d+-\\d+", "step-1-1"),
                    "scenario-\\d+", "scenario-1")));
        }

        static string ScrubLineNumbers(string source)
        {
            var regex = new Regex(@":line \d+");
            return regex.Replace(source, string.Empty);
        }

        static string ScrubPaths(string source)
        {
            var result = Regex.Replace(source, @"(at .* in) .*[\\/](.*.cs)", "$1 ...\\$2", RegexOptions.Multiline);
            return result;
        }
    }
}