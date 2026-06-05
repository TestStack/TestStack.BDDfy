namespace TestStack.BDDfy.Reporters.Html
{
    public class HtmlReporter(HtmlReportConfiguration configuration): IBatchProcessor
    {
        public HtmlReportConfiguration Configuration = configuration;

        public HtmlReportModel Model { get; private set; } = null!;

        public HtmlReporter(IReportBuilder reportBuilder) : this(new HtmlReportConfiguration { ReportBuilder = reportBuilder }) { }
        public HtmlReporter() : this(new HtmlReportConfiguration()) { }

        public void Process(IEnumerable<Story> stories)
        {
            var allowedStories = stories.Where(Configuration.RunsOn).ToList();
            Model = new HtmlReportModel(Configuration, allowedStories.ToReportModel());
            WriteOutHtmlReport();
        }

        void WriteOutHtmlReport()
        {           
            LoadCustomScripts();
            string report;

            try
            {
                report = Configuration.ReportBuilder.CreateReport(Model);
            }
            catch (Exception ex)
            {
                report = ex.Message + ex.StackTrace;
            }

            Configuration.FileWriter.OutputReport(report, Configuration.OutputFileName, Configuration.OutputPath);
        }

        private void LoadCustomScripts()
        {
            var customStylesheet = FileHelpers.ResolvePath(Configuration.OutputPath, "BDDfyCustom.css");

            if (Configuration.FileReader.Exists(customStylesheet))
                Model.CustomStylesheet = Configuration.FileReader.Read(customStylesheet);

            var customJavascript = FileHelpers.ResolvePath(Configuration.OutputPath, "BDDfyCustom.js");
            if (Configuration.FileReader.Exists(customJavascript))
                Model.CustomJavascript = Configuration.FileReader.Read(customJavascript);
        }
    }
}