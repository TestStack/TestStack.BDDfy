﻿// Copyright (C) 2011, Mehdi Khalili
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
using System.Linq;
using System.Web.Script.Serialization;
using TestStack.BDDfy.Core;

namespace TestStack.BDDfy.Processors.Diagnostics
{
    public class DiagnosticsReporter : IBatchProcessor
    {
        private static string OutputDirectory
        {
            get
            {
                string codeBase = typeof(DiagnosticsReporter).Assembly.CodeBase;
                var uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public void Process(IEnumerable<Story> stories)
        {
            const string error = "There was an error compiling the json report: ";
            var viewModel = new FileReportModel(stories);
            string report;

            try
            {
                report = CreateJson(viewModel);
            }
            catch (Exception ex)
            {
                report = error + ex.Message;
            }

            var path = Path.Combine(OutputDirectory, "Diagnostics.json");

            if (File.Exists(path))
                File.Delete(path);
            File.WriteAllText(path, report);
        }

        public string CreateJson(FileReportModel viewModel)
        {
            var graph = new List<object>();
            foreach (var story in viewModel.Stories)
            {
                graph.Add(new
                {
                    StoryName = story.MetaData.Title,
                    StoryDuration = story.Scenarios.Sum(x => x.Duration.Milliseconds),
                    Scenarios = story.Scenarios.Select(scenario => new
                    {
                        ScenarioName = scenario.Title,
                        ScenarioDuration = scenario.Duration.Milliseconds,
                        Steps = scenario.Steps.Select(step => new
                        {
                            StepName = step.StepTitle,
                            StepDuration = step.Duration.Milliseconds
                        })
                    })
                });
            }

            var serializer = new JavaScriptSerializer();
            string json = serializer.Serialize(graph);

            return new JsonFormatter(json).Format();
        }
    }
}
