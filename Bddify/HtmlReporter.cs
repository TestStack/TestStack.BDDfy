using System;
using System.Collections.Generic;
using System.IO;
using RazorEngine;

namespace Bddify
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

        string GetTemplateFile(Bddee bddee)
        {
            var templatefile = Path.Combine(Environment.CurrentDirectory, "HtmlReport.cshtml");
            return File.ReadAllText(templatefile);
        }

        private void CreateHtmlFile(Bddee bddee)
        {
            // ToDo: this is a dirty hack because I am creating the file each and every time.
            // should create the file once and dynamically edit it for consequent calls
            _bddees.Add(bddee);
            var fileName = Path.Combine(_filePath, "bddify.html");

            var report = Razor.Parse(GetTemplateFile(bddee), _bddees);
            File.WriteAllText(fileName, report);
        }
    }
}