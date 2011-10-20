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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Bddify.Core;
using RazorEngine;
using RazorEngine.Templating;

namespace Bddify.Reporters
{
    public class HtmlReportTraceListener : TextWriterTraceListener
    {
        static readonly Dictionary<string, List<Story>> Stories = new Dictionary<string, List<Story>>();
        static readonly object SyncRoot = new object();

        static HtmlReportTraceListener()
        {
            AppDomain.CurrentDomain.DomainUnload += CurrentDomain_DomainUnload;
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            var story = data as Story;
            if (story == null)
                return;

            lock (SyncRoot)
            {
                if (!Stories.ContainsKey(story.Category))
                    Stories[story.Category] = new List<Story>();

                Stories[story.Category].Add(story);
            }
        }

        static void CurrentDomain_DomainUnload(object sender, EventArgs e)
        {
            GenerateHtmlReport(Stories);
        }

        private static void GenerateHtmlReport(Dictionary<string, List<Story>> stories)
        {
            const string error = "There was an error compiling the template";
            var cssFullFileName = Path.Combine(AssemblyDirectory, "bddify.css");
            // create the css file only if it does not already exists. This allows devs to overwrite the css file in their test project
            if(!File.Exists(cssFullFileName))
                File.WriteAllText(cssFullFileName, CssFile.Value);

            foreach (var file in stories.Keys)
            {
                var storiesInFile = stories[file];
                var htmlFileName = file + ".html";
                var htmlFullFileName = Path.Combine(AssemblyDirectory, htmlFileName);

                string report;
                try
                {
                    report = Razor.Parse(HtmlTemplate.Value, storiesInFile);
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
            var templateResourceStream = typeof(HtmlReportTraceListener).Assembly.GetManifestResourceStream(fileResourceName);
            using (var sr = new StreamReader(templateResourceStream))
            {
                fileContent = sr.ReadToEnd();
            }

            return fileContent;
        }

        // http://stackoverflow.com/questions/52797/c-how-do-i-get-the-path-of-the-assembly-the-code-is-in#answer-283917
        static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }
    }
}
#endif
