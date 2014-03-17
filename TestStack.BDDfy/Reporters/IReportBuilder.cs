namespace TestStack.BDDfy.Reporters
{
    public interface IReportBuilder
    {
        string CreateReport(FileReportModel model);
    }
}