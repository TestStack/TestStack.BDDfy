namespace TestStack.BDDfy.Reporters.Writers
{
    public interface IReportWriter
    {
        void OutputReport(string reportData, string reportName, string outputDirectory = null);
    }
}