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
using Bddify.Core;
using Bddify.Module;

namespace Bddify.Reporters.HtmlReporter
{
    public class HtmlReportMainModule : DefaultModule, IReportModule
    {
        static readonly List<Story> Stories = new List<Story>();

        static HtmlReportMainModule()
        {
            AppDomain.CurrentDomain.DomainUnload += CurrentDomain_DomainUnload;
        }

        public virtual void Report(Story story)
        {
            Stories.Add(story);
        }

        static void CurrentDomain_DomainUnload(object sender, EventArgs e)
        {
            GenerateHtmlReport();
        }

        private static void GenerateHtmlReport()
        {
            StoriesByConfig.ForEach(config =>
            {
                WriteOutScriptFilesFor(config);
                WriteOutHtmlReportFor(config);
            });
        }

        static void WriteOutHtmlReportFor(StoryConfig config)
        {
            const string error = "There was an error compiling the html report: ";
            var htmlFullFileName = Path.Combine(config.HtmlReportConfigurationModule.OutputPath, config.HtmlReportConfigurationModule.OutputFileName);
            var viewModel = new HtmlReportViewModel(config.HtmlReportConfigurationModule, config.Stories);
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

        static void WriteOutScriptFilesFor(StoryConfig config)
        {
            // create the files only if it does not already exists. This allows devs to overwrite the css file in their test project
            var bddifyCssFileName = Path.Combine(config.HtmlReportConfigurationModule.OutputPath, LazyFileLoader.BddifyVersionedFileName + ".css");
            if (!File.Exists(bddifyCssFileName))
                File.WriteAllText(bddifyCssFileName, LazyFileLoader.BddifyCssFile);

            var bddifyJavascriptFileName = Path.Combine(config.HtmlReportConfigurationModule.OutputPath, LazyFileLoader.BddifyVersionedFileName + ".js");
            if (!File.Exists(bddifyJavascriptFileName))
                File.WriteAllText(bddifyJavascriptFileName, LazyFileLoader.BddifyJsFile);
            
            var jqueryFileName = Path.Combine(config.HtmlReportConfigurationModule.OutputPath, "jquery-1.7.1.min.js");
            if (!File.Exists(jqueryFileName))
                File.WriteAllText(jqueryFileName, LazyFileLoader.JQueryFile);
        }

        private static bool OutputFileWouldBeOverwritten(StoryConfig config, List<StoryConfig> filteredList)
        {
            return filteredList.Any(x =>
                    (x.HtmlReportConfigurationModule.OutputPath == config.HtmlReportConfigurationModule.OutputPath) &&
                    (x.HtmlReportConfigurationModule.OutputFileName == config.HtmlReportConfigurationModule.OutputFileName));
        }

        private static StoryConfig ConfigWithUniqueFileName(StoryConfig config, List<StoryConfig> filteredList)
        {
            if (OutputFileWouldBeOverwritten(config, filteredList))
            {
                config.HtmlReportConfigurationModule.OutputFileName = string.Format("{0}.html", config.GetType());
                if (OutputFileWouldBeOverwritten(config, filteredList))
                {
                    string[] split = config.HtmlReportConfigurationModule.OutputFileName.Split('.');
                    config.HtmlReportConfigurationModule.OutputFileName = string.Format("{0}{1}.{2}", split[0], Guid.NewGuid().ToString(), split[1]);
                }
            }
            return config;
        }

        static List<StoryConfig> StoriesByConfig
        {
            get
            {
                var dic = new Dictionary<Type, StoryConfig>();
                foreach (var story in Stories)
                {
                    var config = ModuleFactory.Get<IHtmlReportConfigurationModule>(story);
                    var configType = config.GetType();
                    if (!dic.ContainsKey(configType))
                        dic[configType] = new StoryConfig(config, new List<Story>());

                    dic[configType].Stories.Add(story);
                }

                var filteredList = new List<StoryConfig>();
                dic.Values.ToList()
                    .ForEach(config => filteredList.Add(ConfigWithUniqueFileName(config, filteredList)));
                return filteredList;
            }
        }


    }
}
#endif
