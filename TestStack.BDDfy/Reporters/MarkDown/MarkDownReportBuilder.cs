using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace TestStack.BDDfy.Reporters.MarkDown
{
    public class MarkDownReportBuilder : IReportBuilder
    {
        private readonly List<Exception> _exceptions = new List<Exception>();

        public string CreateReport(FileReportModel model)
        {
            var report = new StringBuilder();

            foreach (var story in model.Stories)
            {
                _exceptions.Clear();
                if (story.Metadata != null)
                {
                    report.AppendLine(string.Format("## {0}{1}", story.Metadata.TitlePrefix, story.Metadata.Title));
                    if (!string.IsNullOrEmpty(story.Metadata.Narrative1))
                        report.AppendLine(string.Format(" **{0}**  ", story.Metadata.Narrative1));
                    if (!string.IsNullOrEmpty(story.Metadata.Narrative2))
                        report.AppendLine(string.Format(" **{0}**  ", story.Metadata.Narrative2));
                    if (!string.IsNullOrEmpty(story.Metadata.Narrative3))
                        report.AppendLine(string.Format(" **{0}**  ", story.Metadata.Narrative3));
                }

                report.AppendLine(); // separator

                foreach (var scenarioGroup in story.Scenarios.GroupBy(s => s.Id))
                {
                    if (scenarioGroup.Count() > 1)
                    {
                        // all scenarios in an example based scenario share the same header and narrative
                        var exampleScenario = story.Scenarios.First();
                        report.AppendLine(string.Format("### {0}", exampleScenario.Title));

                        if (exampleScenario.Steps.Any())
                        {
                            foreach (var step in exampleScenario.Steps.Where(s => s.ShouldReport))
                                report.AppendLine("  " + HttpUtility.HtmlEncode(step.Title) + "  ");
                        }

                        report.AppendLine(); // separator
                        WriteExamples(report, exampleScenario, scenarioGroup);
                    }
                    else
                    {
                        foreach (var scenario in scenarioGroup)
                        {
                            report.AppendLine(string.Format("### {0}", scenario.Title));

                            foreach (var step in scenario.Steps)
                                report.AppendLine("  " + HttpUtility.HtmlEncode(step.Title) + "  ");

                            report.AppendLine(); // separator
                        }
                    }
                }

                ReportExceptions(report);
            }
            _exceptions.Clear();

            return report.ToString();
        }

        private void WriteExamples(StringBuilder report, Scenario exampleScenario, IEnumerable<Scenario> scenarioGroup)
        {
            report.AppendLine("### Examples: ");
            report.AppendLine();
            var scenarios = scenarioGroup.ToArray();
            var allPassed = scenarios.All(s => s.Result == Result.Passed);
            var exampleColumns = exampleScenario.Example.Headers.Length;
            var numberColumns = allPassed ? exampleColumns : exampleColumns + 2;
            var maxWidth = new int[numberColumns];
            var rows = new List<string[]>();

            Action<IEnumerable<string>, string, string> addRow = (cells, result, error) =>
            {
                var row = new string[numberColumns];
                var index = 0;

                foreach (var cellText in cells)
                    row[index++] = cellText;

                if (!allPassed)
                {
                    row[numberColumns - 2] = result;
                    row[numberColumns - 1] = error;
                }

                for (var i = 0; i < numberColumns; i++)
                {
                    var rowValue = row[i];
                    if (rowValue != null && rowValue.Length > maxWidth[i])
                        maxWidth[i] = rowValue.Length;
                }

                rows.Add(row);
            };

            addRow(exampleScenario.Example.Headers, "Result", "Errors");
            foreach (var scenario in scenarios)
            {
                var failingStep = scenario.Steps.FirstOrDefault(s => s.Result == Result.Failed);
                var error = failingStep == null
                    ? null
                    : string.Format("Step: {0} failed with exception: {1}", HttpUtility.HtmlEncode(failingStep.Title), CreateExceptionMessage(failingStep));

                addRow(scenario.Example.Values.Select(e => e.GetValueAsString()), scenario.Result.ToString(), error);
            }

            foreach (var row in rows)
                WriteExampleRow(report, row, maxWidth);
        }

        private void WriteExampleRow(StringBuilder report, string[] row, int[] maxWidth)
        {
            report.Append("    ");
            for (int index = 0; index < row.Length; index++)
            {
                var col = row[index];
                report.AppendFormat("| {0} ", (col ?? string.Empty).Trim().PadRight(maxWidth[index]));
            }
            report.AppendLine("|");
        }

        private string CreateExceptionMessage(Step step)
        {
            _exceptions.Add(step.Exception);

            var exceptionReference = string.Format("[Details at {0} below]", _exceptions.Count);
            if (!string.IsNullOrEmpty(step.Exception.Message))
                return string.Format("[{0}] {1}", FlattenExceptionMessage(step.Exception.Message), exceptionReference);

            return string.Format("{0}", exceptionReference);
        }

        void ReportExceptions(StringBuilder report)
        {
            if (_exceptions.Count == 0)
                return;

            report.AppendLine();
            report.Append("#### Exceptions:");

            for (int index = 0; index < _exceptions.Count; index++)
            {
                var exception = _exceptions[index];
                report.AppendLine();
                report.AppendFormat("    {0}. ", index + 1);

                if (!string.IsNullOrEmpty(exception.Message))
                {
                    report.AppendLine(FlattenExceptionMessage(exception.Message));
                }
                else
                    report.AppendLine();

                var stackTrace = string.Join(Environment.NewLine, exception.StackTrace.Split(new[] { Environment.NewLine }, StringSplitOptions.None)
                    .Select(s => "    " + s));
                report.AppendLine(stackTrace);
            }

            report.AppendLine();
        }

        static string FlattenExceptionMessage(string message)
        {
            return message
                .Replace("\t", " ") // replace tab with one space
                .Replace(Environment.NewLine, ", ") // replace new line with one space
                .Trim() // trim starting and trailing spaces
                .Replace("  ", " ")
                .TrimEnd(','); // chop any , from the end
        }
    }
}