using System;
using System.IO;
using TestStack.BDDfy.Processors.Reports.Diagnostics;

namespace TestStack.BDDfy.Processors.Reports.Writers
{
    public class FileWriter : IReportWriter
    {
        public void OutputReport(string reportData, string reportName)
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