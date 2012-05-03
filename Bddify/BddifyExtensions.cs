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
using Bddify.Configuration;
using Bddify.Core;
using Bddify.Scanners;
using Bddify.Scanners.ScenarioScanners;
using System.Linq;

namespace Bddify
{
    public static class BddifyExtensions
    {
        static IScanner GetReflectiveScanner(object testObject, string scenarioTitle = null, Type explicitStoryType = null)
        {
            var stepScanners = Configurator.Scanners.GetStepScanners(testObject).ToArray();
            var reflectiveScenarioScanner = new ReflectiveScenarioScanner(scenarioTitle, stepScanners);

            return new DefaultScanner(testObject, reflectiveScenarioScanner, explicitStoryType);
        }

        /// <summary>
        /// Extension method to Bddify an object instance.
        /// </summary>
        /// <param name="testObject">The test object representing a scenario.</param>
        /// <returns></returns>
        public static Story Bddify(this object testObject)
        {
            return Bddify(testObject, null);
        }

        /// <summary>
        /// Extension method to Bddify an object instance.
        /// </summary>
        /// <param name="testObject">The test object representing a scenario.</param>
        /// <param name="scenarioTitle">Overrides the default scenario title and is displayed in the reports.</param>
        /// <returns></returns>
        public static Story Bddify(this object testObject, string scenarioTitle)
        {
            return testObject.LazyBddify(scenarioTitle).Run();
        }

        /// <summary>
        /// Extension method to Bddify an object instance.
        /// </summary>
        /// <param name="testObject">The test object representing a scenario.</param>
        /// <param name="scenarioTitle">Overrides the default scenario title and is displayed in the reports.</param>
        /// <param name="storyCategory">Used for filename in Html reports. Has no effect on console reports.</param>
        /// <returns></returns>
        public static Story Bddify(this object testObject, string scenarioTitle, string storyCategory)
        {
            return testObject.LazyBddify(scenarioTitle, storyCategory).Run();
        }

        public static Engine LazyBddify(this object testObject, string scenarioTitle = null, string storyCategory = null)
        {
            return InternalLazyBddify(testObject, scenarioTitle, storyCategory);
        }

        /// <summary>
        /// Extension method to Bddify an object instance.
        /// </summary>
        /// <typeparam name="TStory">The type representing the story.</typeparam>
        /// <param name="testObject">The test object representing a scenario.</param>
        /// <returns></returns>
        public static Story Bddify<TStory>(this object testObject)
            where TStory : class
        {
            return Bddify<TStory>(testObject, null);
        }

        /// <summary>
        /// Extension method to Bddify an object instance.
        /// </summary>
        /// <typeparam name="TStory">The type representing the story.</typeparam>
        /// <param name="testObject">The test object representing a scenario.</param>
        /// <param name="scenarioTitle">Overrides the default scenario title and is displayed in the reports.</param>
        /// <returns></returns>
        public static Story Bddify<TStory>(this object testObject, string scenarioTitle)
            where TStory : class
        {
            return testObject.LazyBddify<TStory>(scenarioTitle).Run();
        }

        /// <summary>
        /// Extension method to Bddify an object instance.
        /// </summary>
        /// <typeparam name="TStory">The type representing the story.</typeparam>
        /// <param name="testObject">The test object representing a scenario.</param>
        /// <param name="scenarioTitle">Overrides the default scenario title and is displayed in the reports.</param>
        /// <param name="storyCategory">Used for filename in Html reports. Has no effect on console reports.</param>
        /// <returns></returns>
        public static Story Bddify<TStory>(this object testObject, string scenarioTitle, string storyCategory)
            where TStory : class
        {
            return testObject.LazyBddify<TStory>(scenarioTitle, storyCategory).Run();
        }

        public static Engine LazyBddify<TStory>(this object testObject, string scenarioTitle = null, string storyCategory = null)
            where TStory : class
        {
            return InternalLazyBddify(testObject, scenarioTitle, storyCategory, typeof(TStory));
        }

        static Engine InternalLazyBddify(
            object testObject, 
            string scenarioTitle, 
            string storyCategory,
            Type explicitStoryType = null)
        {
            IScanner scanner = null;
            var hasScanner = testObject as IHasScanner;

            if (hasScanner != null)
            {
                scanner = hasScanner.GetScanner(scenarioTitle, explicitStoryType);
                testObject = hasScanner.TestObject;
            }

            var storyScanner = scanner ?? GetReflectiveScanner(testObject, scenarioTitle, explicitStoryType);

            return new Engine(storyCategory, storyScanner);
        }
    }
}