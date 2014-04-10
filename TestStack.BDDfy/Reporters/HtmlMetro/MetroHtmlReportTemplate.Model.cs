using System;
using System.IO;
using System.Reflection;
using TestStack.BDDfy.Reporters.Html;

namespace TestStack.BDDfy.Reporters.HtmlMetro
{
    /// <summary>
    /// Partial class for the T4 runtime template where it's data comes from
    /// </summary>
    partial class MetroHtmlReportTemplate
    {
        private readonly HtmlReportViewModel _model;
        public DateTime RunDate { get; set; }

        public MetroHtmlReportTemplate(HtmlReportViewModel model, DateTime runDate)
        {
            _model = model;
            RunDate = runDate;
        }

        public HtmlReportViewModel Model
        {
            get { return _model; }
        }

        public string ReportCss
        {
            get
            {
                var assembly = Assembly.GetExecutingAssembly();
                const string resourceName = "TestStack.BDDfy.Reporters.HtmlMetro.BDDfyMetro.min.css";

                using (var stream = assembly.GetManifestResourceStream(resourceName))
                using (var reader = new StreamReader(stream))
                {
                    return  reader.ReadToEnd();
                }
            }
        }

        public string ReportJs
        {
            get
            {
                var assembly = Assembly.GetExecutingAssembly();
                const string resourceName = "TestStack.BDDfy.Reporters.HtmlMetro.BDDfyMetro.min.js";

                using (var stream = assembly.GetManifestResourceStream(resourceName))
                using (var reader = new StreamReader(stream))
                {
                    // the first line contains the min js, other lines contain source map comments which we don't want
                    return reader.ReadLine();
                    
                }
            }
        }
    }
}