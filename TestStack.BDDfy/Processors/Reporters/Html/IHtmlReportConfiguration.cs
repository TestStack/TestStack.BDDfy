using TestStack.BDDfy.Core;

namespace TestStack.BDDfy.Processors.Reporters.Html
{
    public interface IHtmlReportConfiguration
    {
        string ReportHeader { get; }
        string ReportDescription { get; }
        string OutputPath { get; }
        string OutputFileName { get; }
        bool RunsOn(Story story);
    }
}