using System;
using System.IO;
using TestStack.BDDfy.Reporters.Diagnostics;

namespace TestStack.BDDfy.Reporters.Writers
{
    public class FileWriter : IReportWriter
    {
        public void OutputReport(string reportData, string reportName, string outputDirectory = null)
        {
            string directory = outputDirectory ?? FileHelpers.AssemblyDirectory();
            var path = Path.Combine(directory, reportName);

            if (File.Exists(path))
                File.Delete(path);
            File.WriteAllText(path, reportData);
        }
    }
}