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
                if (story.MetaData != null)
                {
                    report.AppendLine(string.Format("## Story: {0}", story.MetaData.Title));
                    report.AppendLine(string.Format(" **{0}**  ", story.MetaData.AsA));
                    report.AppendLine(string.Format(" **{0}**  ", story.MetaData.IWant));
                    report.AppendLine(string.Format(" **{0}**  ", story.MetaData.SoThat));
                }

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