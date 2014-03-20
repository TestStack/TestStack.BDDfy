using System.Text;

namespace TestStack.BDDfy.Reporters.MarkDown
{
    public class MarkDownReportBuilder : IReportBuilder
    {
        public string CreateReport(FileReportModel model)
        {
            var report = new StringBuilder();

            foreach (var story in model.Stories)
            {
                if (story.Metadata != null)
                {
                    report.AppendLine(string.Format("## Story: {0}", story.Metadata.Title));
                    report.AppendLine(string.Format(" **{0}**  ", story.Metadata.AsA));
                    report.AppendLine(string.Format(" **{0}**  ", story.Metadata.IWant));
                    report.AppendLine(string.Format(" **{0}**  ", story.Metadata.SoThat));
                }

                report.AppendLine(); // separator
            
                foreach (var scenario in story.Scenarios)
                {
                    report.AppendLine(string.Format("### {0}", scenario.Title));

                    foreach (var step in scenario.Steps)
                        report.AppendLine("  " + step.Title + "  ");

                    report.AppendLine(); // separator
                }
            }

            return report.ToString();
        }
    }
}