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
using System.Linq;
using Bddify.Configuration;
using Bddify.Processors;

namespace Bddify.Core
{
    public class Bddifier
    {
        private readonly string _storyCategory;
        private readonly IScanner _scanner;

        static Bddifier()
        {
            AppDomain.CurrentDomain.DomainUnload += CurrentDomain_DomainUnload;
        }

        static void CurrentDomain_DomainUnload(object sender, EventArgs e)
        {
            foreach (var batchProcessor in Factory.BatchProcessors.GetProcessors())
            {
                batchProcessor.Process(StoryCache.Stories);
            }
        }

        public Bddifier(string storyCategory, IScanner scanner)
        {
            _storyCategory = storyCategory ?? "bddify";
            _scanner = scanner;
        }

        public Story Run()
        {
            _story = _scanner.Scan();
            _story.Category = _storyCategory;

            var processors = Factory.ProcessorPipeline.GetProcessors(_story).ToList();

            try
            {
                //run processors in the right order regardless of the order they are provided to the Bddifer
                foreach (var processor in processors.Where(p => p.ProcessType != ProcessType.Finally).OrderBy(p => (int)p.ProcessType))
                    processor.Process(_story);
            }
            finally
            {
                foreach (var finallyProcessor in processors.Where(p => p.ProcessType == ProcessType.Finally))
                    finallyProcessor.Process(_story);
            }

            return _story;
        }

        private Story _story;

        public Story Story
        {
            get { return _story; }
        }
    }
}