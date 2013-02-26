namespace TestStack.BDDfy.Processors.Reporters
{
    public interface IReportBuilder
    {
        string CreateReport(FileReportModel model);
    }
}