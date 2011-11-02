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
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

#if !(NET35 || SILVERLIGHT)
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Bddify.Core;
using Bddify.Module;
using RazorEngine;
using RazorEngine.Templating;

namespace Bddify.Reporters
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

        static IDictionary<IHtmlReportConfigurationModule, List<Story>> StoriesByConfig
        {
            get
            {
                var dic = new Dictionary<Type, Tuple<IHtmlReportConfigurationModule, List<Story>>>();
                foreach (var story in Stories)
                {
                    var config = ModuleFactory.Get<IHtmlReportConfigurationModule>(story);
                    var configType = config.GetType();
                    if(!dic.ContainsKey(configType))
                        dic[configType] = new Tuple<IHtmlReportConfigurationModule, List<Story>>(config, new List<Story>());
                
                    dic[configType].Item2.Add(story);
                }

                return dic.ToDictionary(tuple => tuple.Value.Item1, tuple => tuple.Value.Item2);
            }
        }

        private static void GenerateHtmlReport()
        {
            const string error = "There was an error compiling the template";
            
            foreach (var config in StoriesByConfig)
            {
                var cssFullFileName = Path.Combine(config.Key.OutputPath, "bddify.css");
                // create the css file only if it does not already exists. This allows devs to overwrite the css file in their test project
                if (!File.Exists(cssFullFileName))
                    File.WriteAllText(cssFullFileName, CssFile.Value);

                var htmlFullFileName = Path.Combine(config.Key.OutputPath, config.Key.OutputFileName);
                var viewModel = new HtmlReportViewModel(config.Key, config.Value);
                string report;

                try
                {
                    report = Razor.Parse(HtmlTemplate.Value, viewModel);
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
            var templateResourceStream = typeof(HtmlReportMainModule).Assembly.GetManifestResourceStream(fileResourceName);
            using (var sr = new StreamReader(templateResourceStream))
            {
                fileContent = sr.ReadToEnd();
            }

            return fileContent;
        }
    }
}
#endif