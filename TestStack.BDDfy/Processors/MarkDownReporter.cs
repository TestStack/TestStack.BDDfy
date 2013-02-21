using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TestStack.BDDfy.Core;

namespace TestStack.BDDfy.Processors
{
    /// <summary>
    /// This is a custom reporter that shows you how easily you can create a custom report.
    /// Just implement IProcessor and you are done
    /// </summary>
    public class MarkDownReporter : IBatchProcessor
    {
        private static string OutputDirectory
        {
            get
            {
                string codeBase = typeof(MarkDownReporter).Assembly.CodeBase;
                var uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public void Process(IEnumerable<Story> stories)
        {
            const string error = "There was an error compiling the html report: ";
            var viewModel = new FileReportModel(stories);
            string report;

            try
            {
                report = MarkDownReport(viewModel);
            }
            catch (Exception ex)
            {
                report = error + ex.Message;
            }

            var path = Path.Combine(OutputDirectory, "BDDfy.md");

            if (File.Exists(path))
                File.Delete(path);
            File.WriteAllText(path, report);
        }

        private static string MarkDownReport(FileReportModel viewModel)
        {
            var report = new StringBuilder();

            foreach (var story in viewModel.Stories)
            {
                report.AppendLine(string.Format("## Story: {0}", story.MetaData.Title));
                report.AppendLine(string.Format(" **{0}**  ", story.MetaData.AsA));
                report.AppendLine(string.Format(" **{0}**  ", story.MetaData.IWant));
                report.AppendLine(string.Format(" **{0}**  ", story.MetaData.SoThat));
                report.AppendLine(); // separator

                foreach (var scenario in story.Scenarios)
                {
                    report.AppendLine(string.Format("### {0}", scenario.Title));

                    foreach (var step in scenario.Steps)
                        report.AppendLine("  " + step.StepTitle + "  ");

                    report.AppendLine(); // separator
                }
            }

            return report.ToString();
        }
    }
}
