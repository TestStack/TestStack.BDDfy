namespace TestStack.BDDfy.Processors.Reports
{
    public interface IReportBuilder
    {
        string CreateReport(FileReportModel model);
    }
}