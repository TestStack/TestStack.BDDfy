using System;
using System.IO;

namespace TestStack.BDDfy.Processors.Diagnostics
{
    public class FileWriter : IReportWriter
    {
        public void Create(string reportData, string reportName)
        {
            var path = Path.Combine(OutputDirectory, reportName);

            if (File.Exists(path))
                File.Delete(path);
            File.WriteAllText(path, reportData);
        }

        private static string OutputDirectory
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