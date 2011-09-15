#if !(NET35 || SILVERLIGHT)
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Bddify.Core;
using RazorEngine;
using RazorEngine.Templating;

namespace Bddify.Reporters
{
    public class HtmlReportTraceListener : TraceListener
    {
        static readonly Dictionary<string, List<Story>> Stories = new Dictionary<string, List<Story>>();
        static readonly object SyncRoot = new object();
        private readonly string _reportName;

        static HtmlReportTraceListener()
        {
            AppDomain.CurrentDomain.DomainUnload += CurrentDomain_DomainUnload;
        }

        public HtmlReportTraceListener() : this(null)
        {
        }

        public HtmlReportTraceListener(string reportName)
        {
            _reportName = reportName ?? "bddify";
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            var story = data as Story;
            if (story == null)
                return;

            lock (SyncRoot)
            {
                if (!Stories.ContainsKey(_reportName))
                    Stories[_reportName] = new List<Story>();

                Stories[_reportName].Add(story);
            }
        }

        static void CurrentDomain_DomainUnload(object sender, EventArgs e)
        {
            GenerateHtmlReport(Stories);
        }

        private static void GenerateHtmlReport(Dictionary<string, List<Story>> stories)
        {
            const string error = "There was an error compiling the template";
            var cssFullFileName = Path.Combine(AssemblyDirectory, "bddify.css");
            File.WriteAllText(cssFullFileName, CssFile.Value);

            foreach (var file in stories.Keys)
            {
                var storiesInFile = stories[file];
                var htmlFileName = file + ".html";
                var htmlFullFileName = Path.Combine(AssemblyDirectory, htmlFileName);

                string report;
                try
                {
                    report = Razor.Parse(HtmlTemplate.Value, storiesInFile);
                }
                catch (TemplateCompilationException compilationException)
                {
                    if (compilationException.Errors.Count > 0)
                    {
                        var compilerError = compilationException.Errors.First();
                        var reportBuilder = new StringBuilder();
                        reportBuilder.AppendLine(error);
                        reportBuilder.AppendLine("Line No: " + compilerError.Line);
                        reportBuilder.AppendLine("Message: " + compilerError.ErrorText);
                        report = reportBuilder.ToString();
                    }
                    else
                    {
                        report = error + " :: " + compilationException.Message;
                    }
                }
                catch (Exception ex)
                {
                    report = ex.Message;
                }

                File.WriteAllText(htmlFullFileName, report);
            }
        }

        static readonly Lazy<string> HtmlTemplate = new Lazy<string>(() => GetEmbeddedFileResource("Bddify.Reporters.HtmlReport.cshtml"));
        static readonly Lazy<string> CssFile = new Lazy<string>(() => GetEmbeddedFileResource("Bddify.Reporters.bddify.css"));

        static string GetEmbeddedFileResource(string fileResourceName)
        {
            string fileContent;
            var templateResourceStream = typeof(HtmlReportTraceListener).Assembly.GetManifestResourceStream(fileResourceName);
            using (var sr = new StreamReader(templateResourceStream))
            {
                fileContent = sr.ReadToEnd();
            }

            return fileContent;
        }

        // http://stackoverflow.com/questions/52797/c-how-do-i-get-the-path-of-the-assembly-the-code-is-in#answer-283917
        static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public override void Write(string message)
        {
        }

        public override void WriteLine(string message)
        {
        }
    }
}
#endif
