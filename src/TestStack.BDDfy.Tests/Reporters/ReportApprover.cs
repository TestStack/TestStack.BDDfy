#if Approvals
using System;
using System.Linq;
using System.Text.RegularExpressions;
using Shouldly;
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
                result.ShouldMatchApproved(c => c
                    .WithScrubber(s => StackTraceScrubber.ScrubLineNumbers(StackTraceScrubber.ScrubPaths(s)))
                    .UseCallerLocation());
            }
        }

        static string Scrub(string scrubPaths)
        {
            return Regex.Replace(Regex.Replace(Regex.Replace(scrubPaths, @"(?<!\r)\n", "\r\n"), "step-\\d+-\\d+", "step-1-1"), "scenario-\\d+", "scenario-1");
        }
    }

    public static class StackTraceScrubber
    {
        public static string ScrubAnonymousIds(string source)
        {
            var regex = new Regex(@"\w+__\w+");
            return regex.Replace(source, string.Empty);
        }

        public static string ScrubLineNumbers(string source)
        {
            var regex = new Regex(@":line \d+");
            return regex.Replace(source, string.Empty);
        }

        static Regex windowsPathRegex = new Regex(@"\b\w:[\\\w.\s-]+\\");
        static Regex unixPathRegex = new Regex(@"\/[\/\w.\s-]+\/");
        public static string ScrubPaths(string source)
        {
            var result = windowsPathRegex.Replace(source, "...\\");
            result = unixPathRegex.Replace(result, ".../");
            return result;
        }

        public static string ScrubStackTrace(this string text)
        {
            return ScrubberUtils.Combine(ScrubAnonymousIds, ScrubPaths, ScrubLineNumbers)(text);
        }

        public static string Scrub(this Exception exception)
        {
            return ("" + exception).ScrubStackTrace();
        }
    }
    public class ScrubberUtils
    {
        public static Func<string, string> Combine(params Func<string, string>[] scrubbers)
        {
            return (inputText) => scrubbers.Aggregate(inputText, (c, s) => s(c));
        }

        public static Func<string, string> NO_SCRUBBER = (s) => s;

        public static Func<string, string> RemoveLinesContaining(string value)
        {
            return s => string.Join("\n", s.Split('\n').Where(l => !l.Contains(value)));
        }
    }


}
#endif