using System;
using System.IO;

namespace TestStack.BDDfy.Reporters.Writers
{
    public class FileWriter : IReportWriter
    {
        public void OutputReport(string reportData, string reportName, string? outputDirectory = null)
        {
            var filePath = Path.Combine(outputDirectory ?? FileHelpers.AssemblyDirectory(), reportName);
            string directory = Path.GetDirectoryName(filePath) ?? throw new InvalidOperationException("Unable to determine directory.");

            Directory.CreateDirectory(directory);

            File.WriteAllText(filePath, reportData);
        }
    }
}