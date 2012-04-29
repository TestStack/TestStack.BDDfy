// Copyright (C) 2011, Mehdi Khalili
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//     * Redistributions of source code must retain the above copyright
//       notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright
//       notice, this list of conditions and the following disclaimer in the
//       documentation and/or other materials provided with the distribution.
//     * Neither the name of the <organization> nor the
//       names of its contributors may be used to endorse or promote products
//       derived from this software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
// DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCULREMENT OF SUBSTITUTE GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

#if !(SILVERLIGHT)
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bddify.Configuration;
using Bddify.Core;

namespace Bddify.Reporters.HtmlReporter
{
    public class HtmlReporter : IProcessor
    {
        static readonly List<Story> Stories = new List<Story>();

        static HtmlReporter()
        {
            AppDomain.CurrentDomain.DomainUnload += CurrentDomain_DomainUnload;
        }

        public ProcessType ProcessType
        {
            get { return ProcessType.Report; }
        }

        public virtual void Process(Story story)
        {
            Stories.Add(story);
        }

        static void CurrentDomain_DomainUnload(object sender, EventArgs e)
        {
            GenerateHtmlReport();
        }

        private static void GenerateHtmlReport()
        {
            foreach (var storyConfig in StoriesByConfig)
            {
                WriteOutScriptFilesFor(storyConfig);
                WriteOutHtmlReportFor(storyConfig);
            }
        }

        static void WriteOutHtmlReportFor(StoryConfig config)
        {
            const string error = "There was an error compiling the html report: ";
            var htmlFullFileName = Path.Combine(config.HtmlReportConfiguration.OutputPath, config.HtmlReportConfiguration.OutputFileName);
            var viewModel = new HtmlReportViewModel(config.HtmlReportConfiguration, config.Stories);
            ShouldTheReportUseCustomization(config, viewModel);
            string report;

            try
            {
                report = new HtmlReportBuilder(viewModel).BuildReportHtml();
            }
            catch (Exception ex)
            {
                report = error + ex.Message;
            }

            File.WriteAllText(htmlFullFileName, report);
        }

        private static void ShouldTheReportUseCustomization(StoryConfig config, HtmlReportViewModel viewModel)
        {
            var customStylesheet = Path.Combine(config.HtmlReportConfiguration.OutputPath, "bddifyCustom.css");
            viewModel.UseCustomStylesheet = File.Exists(customStylesheet);

            var customJavascript = Path.Combine(config.HtmlReportConfiguration.OutputPath, "bddifyCustom.js");
            viewModel.UseCustomJavascript = File.Exists(customJavascript);
        }

        static void WriteOutScriptFilesFor(StoryConfig config)
        {
            var cssFullFileName = Path.Combine(config.HtmlReportConfiguration.OutputPath, "bddify.css");
            File.WriteAllText(cssFullFileName, LazyFileLoader.BddifyCssFile);

            var jqueryFullFileName = Path.Combine(config.HtmlReportConfiguration.OutputPath, "jquery-1.7.1.min.js");
            File.WriteAllText(jqueryFullFileName, LazyFileLoader.JQueryFile);

            var bddifyJavascriptFileName = Path.Combine(config.HtmlReportConfiguration.OutputPath, "bddify.js");
            File.WriteAllText(bddifyJavascriptFileName, LazyFileLoader.BddifyJsFile);
        }

        static IEnumerable<StoryConfig> StoriesByConfig
        {
            get
            {
                return Factory.HtmlReportConfigurations
                    .Select(config => new StoryConfig(config, Stories.Where(config.RunsOn).ToList())).ToList();
            }
        }
    }
}
#endif
