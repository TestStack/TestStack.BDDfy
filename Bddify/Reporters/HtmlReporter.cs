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
        static readonly List<Story> Stories = new List<Story>();

        public ProcessType ProcessType
        {
            get { return ProcessType.Report; }
        }

        public void Process(Story story)
        {
            Stories.Add(story);
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

        internal static void GenerateHtmlReport()
        {
            string report;
            const string error = "There was an error compiling the template";

            try
            {
                report = Razor.Parse(HtmlTemplate.Value, Stories);
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

            File.WriteAllText(FileName, report);
        }

        static readonly Lazy<string> HtmlTemplate = new Lazy<string>(GetHtmlTemplate);
        private static readonly string FileName = Path.Combine(AssemblyDirectory, "bddify.html");

        static string GetHtmlTemplate()
        {
            string htmlTemplate;
            var templateResourceStream = typeof(HtmlReporter).Assembly.GetManifestResourceStream("Bddify.Reporters.HtmlReport.cshtml");
            using (var sr = new StreamReader(templateResourceStream))
            {
                htmlTemplate = sr.ReadToEnd();
            }

            return htmlTemplate;
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