namespace TestStack.BDDfy.Reporters.Writers
{
    public class FileWriter : IFileWriter
    {
        public static readonly FileWriter Singleton = new();

        public void WriteContents(string contents, string fileName, string? outputDirectory = null)
        {
            var filePath = FileHelpers.ResolvePath(outputDirectory, fileName);
            string directory = Path.GetDirectoryName(filePath) ?? throw new InvalidOperationException("Unable to determine directory.");

            Directory.CreateDirectory(directory);

            File.WriteAllText(filePath, contents);
        }
    }
}