namespace Bddify.Configuration
{
    public interface IHtmlReportConfiguration : IConfiguration
    {
        string ReportHeader { get; }
        string ReportDescription { get; }
        string OutputPath { get; }
        string OutputFileName { get; }
    }
}