using System;
using System.IO;
using System.Text;
using Bddify.Core;
using Bddify.Module;
using Bddify.Reporters;
using System.Linq;

namespace Bddify.Samples.TicTacToe
{
    /// <summary>
    /// This is a custom reporter that shows you how easily you can create a custom report.
    /// Just implemented IReportModule and you are done. Everything gets resolved automagically for you
    /// </summary>
    public class CustomTextReporter : DefaultModule, IReportModule
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
            Path = System.IO.Path.Combine(OutputDirectory, "bddify-text-report.txt");
            
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

        public override bool RunsOn(Story story)
        {
            // use this report only for tic tac toe stories
            return story.MetaData.Type.Name.Contains("TicTacToe");
        }

        public void Report(Story story)
        {
            var scenario = story.Scenarios.First();
            var scenarioReport = new StringBuilder();
            scenarioReport.AppendLine(string.Format(" SCENARIO: {0}  ", scenario.Title));
            
            if (scenario.Result != StepExecutionResult.Passed)
            {
                scenarioReport.Append(string.Format("    {0} : ", scenario.Result));
                scenarioReport.AppendLine(scenario.Steps.First(s => s.Result == scenario.Result).Exception.Message);
            }

            scenarioReport.AppendLine();

            foreach (var step in scenario.Steps)
                scenarioReport.AppendLine(string.Format("   [{1}] {0}", step.StepTitle, step.Result));

            scenarioReport.AppendLine("--------------------------------------------------------------------------------");
            scenarioReport.AppendLine();

            File.AppendAllText(Path, scenarioReport.ToString());
        }
    }
}