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
using System.Diagnostics;
using System.Linq;
using TestStack.BDDfy.Core;

namespace TestStack.BDDfy.Scanners
{
    public class StoryAttributeMetaDataScanner : IStoryMetaDataScanner
    {
        public virtual StoryMetaData Scan(object testObject, Type explicitStoryType = null)
        {
            return GetStoryMetaData(testObject, explicitStoryType) ?? GetStoryMetaDataFromScenario(testObject);
        }

        static StoryMetaData GetStoryMetaDataFromScenario(object testObject)
        {
            var scenarioType = testObject.GetType();
            var storyAttribute = GetStoryAttribute(scenarioType);
            if (storyAttribute == null)
                return null;

            return new StoryMetaData(scenarioType, storyAttribute);
        }

        StoryMetaData GetStoryMetaData(object testObject, Type explicityStoryType)
        {
            var candidateStoryType = GetCandidateStory(testObject, explicityStoryType);
            if (candidateStoryType == null)
                return null;

            var storyAttribute = GetStoryAttribute(candidateStoryType);
            if (storyAttribute == null)
                return null;

            return new StoryMetaData(candidateStoryType, storyAttribute);
        }

        protected virtual Type GetCandidateStory(object testObject, Type explicitStoryType)
        {
            if (explicitStoryType != null)
                return explicitStoryType;

            var stackTrace = new StackTrace();
            var frames = stackTrace.GetFrames();
            if (frames == null)
                return null;

            var scenarioType = testObject.GetType();
            // This is assuming scenario and story live in the same assembly
            var firstFrame = frames.LastOrDefault(f => f.GetMethod().DeclaringType.Assembly == scenarioType.Assembly);
            if (firstFrame == null)
                return null;

            return firstFrame.GetMethod().DeclaringType;
        }

        static StoryAttribute GetStoryAttribute(Type candidateStoryType)
        {
            return (StoryAttribute)candidateStoryType.GetCustomAttributes(typeof(StoryAttribute), true).FirstOrDefault();
        }
    }
}