namespace TestStack.BDDfy.Reporters.Writers
{
    public interface IFileWriter
    {
        void OutputReport(string reportData, string reportName, string? outputDirectory = null);
    }
}