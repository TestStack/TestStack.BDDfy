using System;
using System.IO;
using System.Reflection;
using Bddify.Module;

namespace Bddify.Reporters
{
    public class DefaultHtmlReportConfigurationModule : DefaultModule, IHtmlReportConfigurationModule
    {
        public virtual string ReportHeader
        {
            get { return "bddify"; }
        }

        public virtual string ReportDescription
        {
            get { return "A simple BDD framework for .Net developers"; }
        }

        public virtual string OutputPath
        {
            get { return AssemblyDirectory; }
        }

        public virtual string OutputFileName
        {
            get { return "bddify.html"; }
        }

        // http://stackoverflow.com/questions/52797/c-how-do-i-get-the-path-of-the-assembly-the-code-is-in#answer-283917
        private static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }
    }
}