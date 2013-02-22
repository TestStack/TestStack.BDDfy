namespace TestStack.BDDfy.Processors.Diagnostics
{
    public interface IReportWriter
    {
        void OutputReport(string reportData, string reportName);
    }
}