using System.Text.RegularExpressions;

namespace TestStack.BDDfy.Tests.Reporters.Html
{
    public static class LineEndingsScrubber
    {
        public static string Scrub(string scrubPaths)
        {
            return Regex.Replace(scrubPaths, @"(?<!\r)\n", "\r\n");
        }
    }
}