using System;
using System.IO;

namespace TestStack.BDDfy.Processors
{
    public class FileWriter : IReportWriter
    {
        public void OutputReport(string reportData, string reportName, string outputDirectory = null)
        {
            string directory = outputDirectory ?? GetDefaultOutputDirectory;
            var path = Path.Combine(directory, reportName);

            if (File.Exists(path))
                File.Delete(path);
            File.WriteAllText(path, reportData);
        }

        private static string GetDefaultOutputDirectory
        {
            get
            {
                string codeBase = typeof(DiagnosticsReporter).Assembly.CodeBase;
                var uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }
    }
}