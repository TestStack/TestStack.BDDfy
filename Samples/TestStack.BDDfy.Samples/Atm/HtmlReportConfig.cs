using TestStack.BDDfy.Processors;
using TestStack.BDDfy.Reporters.Html;

namespace TestStack.BDDfy.Samples.Atm
{
    /// <summary>
    /// This overrides the default html report setting
    /// </summary>
    public class HtmlReportConfig : DefaultHtmlReportConfiguration
    {
        public override bool RunsOn(Story story)
        {
            return story.Metadata.Type.Namespace != null && story.Metadata.Type.Namespace.EndsWith("Atm");
        }

        /// <summary>
        /// Change the output file name
        /// </summary>
        public override string OutputFileName
        {
            get
            {
                return "ATM.html";
            }
        }

        /// <summary>
        /// Change the report header to your project
        /// </summary>
        public override string ReportHeader
        {
            get
            {
                return "ATM Solutions";
            }
        }

        /// <summary>
        /// Change the report description
        /// </summary>
        public override string ReportDescription
        {
            get
            {
                return "A reliable solution for your offline banking needs";
            }
        }
    }
}