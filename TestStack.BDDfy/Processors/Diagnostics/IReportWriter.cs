namespace TestStack.BDDfy.Processors.Diagnostics
{
    public interface IReportWriter
    {
        void Create(string reportData, string reportName);
    }
}