namespace TestStack.BDDfy.Processors
{
    public interface IReportWriter
    {
        void OutputReport(string reportData, string reportName, string outputDirectory = null);
    }
}