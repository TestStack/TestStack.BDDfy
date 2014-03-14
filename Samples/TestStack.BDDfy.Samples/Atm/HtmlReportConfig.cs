using TestStack.BDDfy.Processors;

namespace TestStack.BDDfy.Samples.Atm
{
    /// <summary>
    /// This overrides the default html report setting
    /// </summary>
    public class HtmlReportConfig : DefaultHtmlReportConfiguration
    {
        public override bool RunsOn(Story story)
        {
            return story.MetaData.Type.Namespace != null && story.MetaData.Type.Namespace.EndsWith("Atm");
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