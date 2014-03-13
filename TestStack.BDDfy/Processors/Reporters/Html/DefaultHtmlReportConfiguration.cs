using System;
using System.IO;
using System.Reflection;
using TestStack.BDDfy.Core;

namespace TestStack.BDDfy.Processors
{
    public class DefaultHtmlReportConfiguration : IHtmlReportConfiguration
    {
        public virtual string ReportHeader
        {
            get { return "BDDfy"; }
        }

        public virtual string ReportDescription
        {
            get { return "A simple BDD framework for .Net developers"; }
        }

        public virtual string OutputPath
        {
            get { return AssemblyDirectory; }
        }

        private string _outputFileName = "BDDfy.html";
        public virtual string OutputFileName
        {
            get { return _outputFileName; }
        }

        public virtual bool RunsOn(Story story)
        {
            return true;
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