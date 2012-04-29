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

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Bddify.Core;

namespace Bddify.Reporters.MarkDownReporter
{
    /// <summary>
    /// This is a custom reporter that shows you how easily you can create a custom report.
    /// Just implement IProcessor and you are done
    /// </summary>
    public class MarkDownReportProcessor : IProcessor
    {
        private static string OutputDirectory
        {
            get
            {
                string codeBase = typeof(MarkDownReportProcessor).Assembly.CodeBase;
                var uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        static MarkDownReportProcessor()
        {
            AppDomain.CurrentDomain.DomainUnload += CurrentDomain_DomainUnload;
        }

        public virtual void Process(Story story)
        {
            Stories.Add(story);
        }

        public ProcessType ProcessType
        {
            get { return ProcessType.Report; }
        }

        static readonly List<Story> Stories = new List<Story>();

        static void CurrentDomain_DomainUnload(object sender, EventArgs e)
        {
            GenerateMarkDownReport();
        }

        private static void GenerateMarkDownReport()
        {
            const string error = "There was an error compiling the html report: ";
            var viewModel = new FileReportModel(Stories);
            string report;

            try
            {
                report = MarkDownReport(viewModel);
            }
            catch (Exception ex)
            {
                report = error + ex.Message;
            }

            var path = Path.Combine(OutputDirectory, "bddify.markdown");

            if (File.Exists(path))
                File.Delete(path);
            File.WriteAllText(path, report);
        }

        private static string MarkDownReport(FileReportModel viewModel)
        {
            var report = new StringBuilder();

            foreach (var story in viewModel.Stories)
            {
                report.AppendLine(string.Format("## Story: {0}", story.MetaData.Title));
                report.AppendLine(string.Format(" **{0}**  ", story.MetaData.AsA));
                report.AppendLine(string.Format(" **{0}**  ", story.MetaData.IWant));
                report.AppendLine(string.Format(" **{0}**  ", story.MetaData.SoThat));
                report.AppendLine(); // separator

                foreach (var scenario in story.Scenarios)
                {
                    report.AppendLine(string.Format("### {0}", scenario.Title));

                    foreach (var step in scenario.Steps)
                        report.AppendLine("  " + step.StepTitle + "  ");

                    report.AppendLine(); // separator
                }
            }

            return report.ToString();
        }
    }
}
