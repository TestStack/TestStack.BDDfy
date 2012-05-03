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

using System;
using System.Collections.Generic;
using System.IO;
using Bddify.Core;
using System.Linq;

namespace Bddify.Reporters.HtmlReporter
{
    public class HtmlReporter : IBatchProcessor
    {
        void WriteOutHtmlReport(IEnumerable<Story> stories)
        {
            const string error = "There was an error compiling the html report: ";
            var htmlFullFileName = Path.Combine(_configuration.OutputPath, _configuration.OutputFileName);
            var viewModel = new HtmlReportViewModel(_configuration, stories);
            ShouldTheReportUseCustomization(viewModel);
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

        private void ShouldTheReportUseCustomization(HtmlReportViewModel viewModel)
        {
            var customStylesheet = Path.Combine(_configuration.OutputPath, "bddifyCustom.css");
            viewModel.UseCustomStylesheet = File.Exists(customStylesheet);

            var customJavascript = Path.Combine(_configuration.OutputPath, "bddifyCustom.js");
            viewModel.UseCustomJavascript = File.Exists(customJavascript);
        }

        void WriteOutScriptFiles()
        {
            var cssFullFileName = Path.Combine(_configuration.OutputPath, "bddify.css");
            File.WriteAllText(cssFullFileName, LazyFileLoader.BddifyCssFile);

            var jqueryFullFileName = Path.Combine(_configuration.OutputPath, "jquery-1.7.1.min.js");
            File.WriteAllText(jqueryFullFileName, LazyFileLoader.JQueryFile);

            var bddifyJavascriptFileName = Path.Combine(_configuration.OutputPath, "bddify.js");
            File.WriteAllText(bddifyJavascriptFileName, LazyFileLoader.BddifyJsFile);
        }

        readonly IHtmlReportConfiguration _configuration;

        public HtmlReporter(IHtmlReportConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Process(IEnumerable<Story> stories)
        {
            WriteOutScriptFiles();

            var allowedStories = stories.Where(s => _configuration.RunsOn(s)).ToList();
            WriteOutHtmlReport(allowedStories);
        }
    }
}