using System;
using System.IO;
using System.Linq;
using System.Text;

namespace TestStack.BDDfy.Samples
{
    /// <summary>
    /// This is a custom reporter that shows you how easily you can create a custom report.
    /// Just implemented IProcessor and you are done
    /// </summary>
    public class CustomTextReporter : IProcessor
    {
        private static readonly string Path;

        private static string OutputDirectory
        {
            get
            {
                string codeBase = typeof(CustomTextReporter).Assembly.CodeBase;
                var uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return System.IO.Path.GetDirectoryName(path);
            }
        }

        static CustomTextReporter()
        {
            Path = System.IO.Path.Combine(OutputDirectory, "BDDfy-text-report.txt");
            
            if(File.Exists(Path))
                File.Delete(Path);

            var header = 
                " A custom report created from your test assembly with no required configuration " + 
                Environment.NewLine + 
                Environment.NewLine + 
                Environment.NewLine + 
                Environment.NewLine;
            File.AppendAllText(Path, header);
        }

        public void Process(Story story)
        {
            // use this report only for tic tac toe stories
            if (story.Metadata == null || !story.Metadata.Type.Name.Contains("TicTacToe"))
                return;

            var scenario = story.Scenarios.First();
            var scenarioReport = new StringBuilder();
            scenarioReport.AppendLine(string.Format(" SCENARIO: {0}  ", scenario.Title));

            if (scenario.Result != Result.Passed && scenario.Steps.Any(s => s.Exception != null))
            {
                scenarioReport.Append(string.Format("    {0} : ", scenario.Result));
                scenarioReport.AppendLine(scenario.Steps.First(s => s.Result == scenario.Result).Exception.Message);
            }

            scenarioReport.AppendLine();

            foreach (var step in scenario.Steps)
                scenarioReport.AppendLine(string.Format("   [{1}] {0}", step.Title, step.Result));

            scenarioReport.AppendLine("--------------------------------------------------------------------------------");
            scenarioReport.AppendLine();

            File.AppendAllText(Path, scenarioReport.ToString());
        }

        public ProcessType ProcessType
        {
            get { return ProcessType.Report; }
        }
    }
}