using System;
using System.IO;
using System.Reflection;

namespace TestStack.BDDfy.Reporters.Html
{
    public class DefaultHtmlReportConfiguration : IHtmlReportConfiguration
    {
        private string _reportHeader = "BDDfy";
        public virtual string ReportHeader
        {
            get { return _reportHeader; }
            set { _reportHeader = value; }
        }

        private string _reportDescription = "A simple BDD framework for .Net developers";
        public virtual string ReportDescription
        {
            get { return _reportDescription; } 
            set { _reportDescription = value; }
        }

        private string _outputPath = FileHelpers.AssemblyDirectory();
        public virtual string OutputPath
        {
            get { return _outputPath; }
            set { _outputPath = value; }
        }

        private string _outputFileName = "BDDfy.html";
        public virtual string OutputFileName
        {
            get { return _outputFileName; }
            set { _outputFileName = value; }
        }

        private bool _resolveJqueryFromCdn = true;
        public virtual bool ResolveJqueryFromCdn
        {
            get { return _resolveJqueryFromCdn; }
            set { _resolveJqueryFromCdn = value; }
        }

        public virtual bool RunsOn(Story story)
        {
            return true;
        }
    }
}