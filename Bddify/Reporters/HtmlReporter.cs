using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Bddify.Core;
#if !(NET35 || SILVERLIGHT)
using RazorEngine;
using RazorEngine.Templating;
#endif

namespace Bddify.Reporters
{
    public class HtmlReporter : IProcessor
    {
        static readonly Dictionary<string, List<Story>> Stories = new Dictionary<string, List<Story>>();
        static readonly object SyncRoot = new object();
        private readonly string _htmlReportName;

        public ProcessType ProcessType
        {
            get { return ProcessType.Report; }
        }

        public HtmlReporter(string htmlReportName)
        {
            _htmlReportName = htmlReportName ?? "bddify";
        }

        public void Process(Story story)
        {
            lock (SyncRoot)
            {
                if (!Stories.ContainsKey(_htmlReportName))
                    Stories[_htmlReportName] = new List<Story>();

                Stories[_htmlReportName].Add(story);
            }
        }

#if NET35 || SILVERLIGHT
        internal static void GenerateHtmlReport()
        {
        }
#else
        static HtmlReporter()
        {
            AppDomain.CurrentDomain.DomainUnload += CurrentDomain_DomainUnload;
        }

        static void CurrentDomain_DomainUnload(object sender, EventArgs e)
        {
            // run this only once when the appdomain is unloading, that is when the last test is run
            GenerateHtmlReport();
        }

        private static void GenerateHtmlReport()
        {
            const string error = "There was an error compiling the template";
            var cssFullFileName = Path.Combine(AssemblyDirectory, "bddify.css");
            File.WriteAllText(cssFullFileName, CssFile.Value);

            foreach (var file in Stories.Keys)
            {
                var storiesInFile = Stories[file];
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
            var templateResourceStream = typeof(HtmlReporter).Assembly.GetManifestResourceStream(fileResourceName);
            using (var sr = new StreamReader(templateResourceStream))
            {
                fileContent = sr.ReadToEnd();
            }

            return fileContent;
        }

        // http://stackoverflow.com/questions/52797/c-how-do-i-get-the-path-of-the-assembly-the-code-is-in#answer-283917
        static public string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }
#endif
    }
}