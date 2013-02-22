namespace TestStack.BDDfy.Processors.Reports.Writers
{
    public interface IReportWriter
    {
        void OutputReport(string reportData, string reportName, string outputDirectory = null);
    }
}