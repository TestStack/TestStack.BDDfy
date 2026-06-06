namespace TestStack.BDDfy.Reporters.Writers
{
    public interface IFileWriter
    {
        void WriteContents(string reportData, string reportName, string? outputDirectory = null);
    }
}