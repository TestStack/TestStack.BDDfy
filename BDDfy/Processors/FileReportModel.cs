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

using System.Collections.Generic;
using System.Linq;
using BDDfy.Core;

namespace BDDfy.Processors
{
    public class FileReportModel
    {
        public FileReportModel(IEnumerable<Story> stories)
        {
            _stories = stories;
            Summary = new FileReportSummaryModel(stories);
        }

        readonly IEnumerable<Story> _stories;
        public FileReportSummaryModel Summary { get; private set; }

        public IEnumerable<Story> Stories
        {
            get
            {
                var groupedByNamespace = from story in _stories
                                         where story.MetaData == null
                                         let ns = story.Scenarios.First().TestObject.GetType().Namespace
                                         orderby ns
                                         group story by ns into g
                                         select g;

                var groupedByStories = from story in _stories
                                       where story.MetaData != null
                                       orderby story.MetaData.Title   // order stories by their title
                                       group story by story.MetaData.Type.Name into g
                                       select g;

                var aggregatedStories = from story in groupedByStories.Union(groupedByNamespace)
                                        select new Story(
                                            story.First().MetaData, // first story in the group is a representative for the entire group
                                            story.SelectMany(s => s.Scenarios).OrderBy(s => s.Title).ToArray()); // order scenarios by title

                return aggregatedStories;
            }
        }
    }
}