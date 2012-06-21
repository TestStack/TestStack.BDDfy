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
using System.Linq;
using TestStack.BDDfy.Core;
using TestStack.BDDfy.Processors;

namespace TestStack.BDDfy.Configuration
{
    public class Processors
    {
        IEnumerable<IProcessor> _GetProcessors(Story story)
        {
            var runner = TestRunner.ConstructFor(story);
            if (runner != null)
                yield return runner;

            var consoleReporter = ConsoleReport.ConstructFor(story);
            if (consoleReporter != null)
                yield return consoleReporter;

            yield return new ExceptionProcessor();

            var storyCache = StoryCache.ConstructFor(story);
            if (storyCache != null)
                yield return storyCache;

            yield return new Disposer();

            foreach (var addedProcessor in _addedProcessors)
            {
                yield return addedProcessor();
            }
        }

        private readonly ProcessorFactory _testRunnerFactory = new ProcessorFactory(() => new TestRunner());
        public ProcessorFactory TestRunner { get { return _testRunnerFactory; } }

        private readonly ProcessorFactory _consoleReportFactory = new ProcessorFactory(() => new ConsoleReporter());
        public ProcessorFactory ConsoleReport { get { return _consoleReportFactory; } }

        private readonly ProcessorFactory _storyCacheFactory = new ProcessorFactory(() => new StoryCache());
        public ProcessorFactory StoryCache { get { return _storyCacheFactory; } }

        readonly List<Func<IProcessor>> _addedProcessors = new List<Func<IProcessor>>();

        public Processors Add(Func<IProcessor> processorFactory)
        {
            _addedProcessors.Add(processorFactory);
            return this;
        }

        public IEnumerable<IProcessor> GetProcessors(Story story)
        {
            var pipeline = from processor in _GetProcessors(story)
                           orderby processor.ProcessType
                           select processor;

            return pipeline.ToList();
        }
    }
}