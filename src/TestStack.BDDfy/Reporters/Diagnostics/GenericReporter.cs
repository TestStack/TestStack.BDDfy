using TestStack.BDDfy.Reporters.Html;

namespace TestStack.BDDfy.Reporters.Diagnostics
{
    public class GenericReporter<TReportBuilder>(ReportConfiguration<TReportBuilder> configuration) : IBatchProcessor where TReportBuilder : IReportBuilder, new()
    {
        private readonly ReportConfiguration<TReportBuilder> _configuration = configuration;
        public GenericReporter(string outputFileName) : this(new ReportConfiguration<TReportBuilder>(outputFileName)) { }

        public virtual void Process(IEnumerable<Story> stories)
        {
            var viewModel = new FileReportModel(stories.ToReportModel());
            string report;

            try
            {
                report = _configuration.ReportBuilder.CreateReport(viewModel);
            }
            catch (Exception ex)
            {
                report = ex.Message + ex.StackTrace;
            }

            _configuration.FileWriter.OutputReport(report, _configuration.OutputFileName);
        }
    }
}
