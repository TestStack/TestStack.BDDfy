using System.IO;
using System.Reflection;
using RazorEngine;

namespace Bddify
{
    public class HtmlReporter : IProcessor
    {
        private readonly string _filePath;
        private readonly bool _createCompleteHtml;

        public HtmlReporter(string filePath, bool createCompleteHtml = true)
        {
            _filePath = filePath;
            _createCompleteHtml = createCompleteHtml;
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
            //var templatefile = Path.Combine(Assembly.get.GetExecutingAssembly().Location, "HtmlReport.cshtml");
            var templatefile = @"C:\Users\Mehdi\Documents\_mine\Professional\Blog\SourceCode\Bddify\Bddify\HtmlReport.cshtml";
            return File.ReadAllText(templatefile);
        }

        private void CreateHtmlFile(Bddee bddee)
        {
            var fileName = Path.Combine(_filePath, bddee.Object.GetType().Name + ".html");

            var report = Razor.Parse(GetTemplateFile(bddee), bddee);
            File.WriteAllText(fileName, report);
        }
    }
}