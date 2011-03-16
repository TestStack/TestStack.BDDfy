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

        static string JqueryFile
        {
            get
            {
                return Path.Combine(Environment.CurrentDirectory, "Reporters\\jquery-1.4.4.min.js");
            }
        }

        static string TemplateFile
        {
            get
            {
                var templatefile = Path.Combine(Environment.CurrentDirectory, "Reporters\\HtmlReport.cshtml");
                return File.ReadAllText(templatefile);
            }
        }

        private void CreateHtmlFile(Bddee bddee)
        {
            // ToDo: this is a dirty hack because I am creating the file each and every time.
            // should create the file once and dynamically edit it for consequent calls
            _bddees.Add(bddee);
            var fileName = Path.Combine(_filePath, "bddify.html");
            var jquery = Path.Combine(_filePath, "jquery-1.4.4.min.js");
            if(!File.Exists(jquery))
                File.Copy(JqueryFile, jquery, true);

            var report = Razor.Parse(TemplateFile, _bddees);
            File.WriteAllText(fileName, report);
        }
    }
}