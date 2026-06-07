using System.Net;
using System.Text;
using TestStack.BDDfy.Configuration;

namespace TestStack.BDDfy.Reporters.MarkDown
{
    public class MarkDownReportBuilder : IReportBuilder
    {
        private readonly List<Exception> _exceptions = [];

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

                foreach (var scenario in story.Scenarios)
                {
                    if (scenario.Examples.Count > 0)
                    {
                        report.AppendLine(string.Format("### {0}", scenario.Title));

                        if (scenario.Steps.Count != 0)
                        {
                            foreach (var step in scenario.Steps.Where(s => s.ShouldReport))
                                report.AppendLine("  " + WebUtility.HtmlEncode(step.Title) + "  ");
                        }

                        report.AppendLine(); // separator
                        WriteExamples(report, scenario);
                        ReportTags(report, scenario.Tags);
                    }
                    else
                    {
                        report.AppendLine(string.Format("### {0}", scenario.Title));

                        foreach (var step in scenario.Steps.Where(s => s.ShouldReport))
                            report.AppendLine("  " + WebUtility.HtmlEncode(step.Title) + "  ");

                        report.AppendLine(); // separator
                        ReportTags(report, scenario.Tags);
                    }
                }

                ReportExceptions(report);
            }
            _exceptions.Clear();

            return report.ToString();
        }

        private static void ReportTags(StringBuilder report, List<string> tags)
        {
            if (tags.Count == 0)
                return;

            report.AppendLine(string.Format("Tags: {0}", string.Join(", ", tags.Select(t => string.Format("`{0}`", t)))));
        }

        private void WriteExamples(StringBuilder report, ReportModel.Scenario scenario)
        {
            var firstExample = scenario.Examples[0];

            report.AppendLine("### Examples: ");
            report.AppendLine();
            var allPassed = scenario.Examples.All(e => e.Result == Result.Passed);
            var exampleColumns = firstExample.Headers.Length;
            var numberColumns = allPassed ? exampleColumns : exampleColumns + 2;
            var maxWidth = new int[numberColumns];
            var rows = new List<string[]>();

            void addRow(IEnumerable<string> cells, string result, string? error)
            {
                var row = new string[numberColumns];
                var index = 0;

                foreach (var cellText in cells)
                    row[index++] = cellText;

                if (!allPassed)
                {
                    row[numberColumns - 2] = result;
                    row[numberColumns - 1] = error!;
                }

                for (var i = 0; i < numberColumns; i++)
                {
                    var rowValue = row[i];
                    if (rowValue != null && rowValue.Length > maxWidth[i])
                        maxWidth[i] = rowValue.Length;
                }

                rows.Add(row);
            }

            addRow(firstExample.Headers, "Result", "Errors");
            foreach (var example in scenario.Examples)
            {
                var error = example.Error is null
                    ? null
                    : string.Format("Exception: {0}", CreateExceptionMessage(example.Error));

                addRow(example.Values.Select(e => e.GetValueAsString()), example.Result.ToString(), error);
            }

            foreach (var row in rows)
                WriteExampleRow(report, row, maxWidth);
        }

        private static void WriteExampleRow(StringBuilder report, string[] row, int[] maxWidth)
        {
            report.Append("    ");
            for (int index = 0; index < row.Length; index++)
            {
                var col = row[index];
                report.AppendFormat("| {0} ", (col ?? string.Empty).Trim().PadRight(maxWidth[index]));
            }
            report.AppendLine("|");
        }

        private string? CreateExceptionMessage(Exception? exception)
        {
            if (exception is null) return null;

            _exceptions.Add(exception);

            var exceptionReference = string.Format("[Details at {0} below]", _exceptions.Count);
            if (!string.IsNullOrEmpty(exception.Message))
                return string.Format("[{0}] {1}", Configurator.ExceptionFormatter.Format(exception.Message), exceptionReference);

            return exceptionReference;
        }

        void ReportExceptions(StringBuilder report)
        {
            if (_exceptions.Count == 0)
                return;

            report.AppendLine();
            report.Append("#### Exceptions:").AppendLine();
            report.AppendLine("```");

            for (int index = 0; index < _exceptions.Count; index++)
            {
                var exception = _exceptions[index];
                report.AppendFormat("{0}. ", index + 1);

                var formatted = Configurator.ExceptionFormatter.Format(exception);
                if (formatted.Length > 0)
                {
                    report.AppendLine(formatted);
                }
                else
                    report.AppendLine();
            }

            report.AppendLine("```").AppendLine();
        }
    }
}