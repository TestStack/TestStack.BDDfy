using System;
using System.Collections.Generic;
using System.IO;
using Bddify.Core;
using RazorEngine;

namespace Bddify.Reporters
{
    public class HtmlReporter : IProcessor
    {
        private readonly string _filePath;
        static readonly List<Bddee> _bddees = new List<Bddee>();

        public HtmlReporter(string filePath, bool createCompleteHtml = true)
        {
            _filePath = filePath;
        }

        public ProcessType ProcessType
        {
            get { return ProcessType.Report; }
        }

        public void Process(Bddee bddee)
        {
            CreateHtmlFile(bddee);
        }

        static readonly Lazy<string> HtmlTemplate = new Lazy<string>(GetHtmlTemplate);
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

        private void CreateHtmlFile(Bddee bddee)
        {
            // ToDo: this is a dirty hack because I am creating the file each and every time.
            // should create the file once and dynamically edit it for consequent calls
            _bddees.Add(bddee);
            var fileName = Path.Combine(_filePath, "bddify.html");

            var report = Razor.Parse(HtmlTemplate.Value, _bddees);
            File.WriteAllText(fileName, report);
        }
    }
}