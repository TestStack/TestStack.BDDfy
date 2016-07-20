using System;
using System.IO;
using TestStack.BDDfy.Reporters.Diagnostics;

namespace TestStack.BDDfy.Reporters.Writers
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
                
#if NET40
                string codeBase = typeof(DiagnosticsReporter).Assembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
#else
                var basePath = AppContext.BaseDirectory;
                return Path.GetFullPath(basePath);
#endif
            }
        }
    }
}