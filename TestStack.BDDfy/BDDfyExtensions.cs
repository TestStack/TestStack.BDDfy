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
using TestStack.BDDfy.Configuration;
using TestStack.BDDfy.Core;
using TestStack.BDDfy.Scanners;
using TestStack.BDDfy.Scanners.ScenarioScanners;

namespace TestStack.BDDfy
{
    public static class BDDfyExtensions
    {
        static IScanner GetReflectiveScanner(object testObject, string scenarioTitle = null, Type explicitStoryType = null)
        {
            var stepScanners = Configurator.Scanners.GetStepScanners(testObject).ToArray();
            var reflectiveScenarioScanner = new ReflectiveScenarioScanner(scenarioTitle, stepScanners);

            return new DefaultScanner(testObject, reflectiveScenarioScanner, explicitStoryType);
        }

        /// <summary>
        /// Extension method to BDDfy an object instance.
        /// </summary>
        /// <param name="testObject">The test object representing a scenario.</param>
        /// <returns></returns>
        public static Story BDDfy(this object testObject)
        {
            return BDDfy(testObject, null);
        }

        /// <summary>
        /// Extension method to BDDfy an object instance.
        /// </summary>
        /// <param name="testObject">The test object representing a scenario.</param>
        /// <param name="scenarioTitle">Overrides the default scenario title and is displayed in the reports.</param>
        /// <returns></returns>
        public static Story BDDfy(this object testObject, string scenarioTitle)
        {
            return testObject.LazyBDDfy(scenarioTitle).Run();
        }

        /// <summary>
        /// Extension method to BDDfy an object instance.
        /// </summary>
        /// <param name="testObject">The test object representing a scenario.</param>
        /// <param name="scenarioTitle">Overrides the default scenario title and is displayed in the reports.</param>
        /// <param name="storyCategory">Used for filename in Html reports. Has no effect on console reports.</param>
        /// <returns></returns>
        public static Story BDDfy(this object testObject, string scenarioTitle, string storyCategory)
        {
            return testObject.LazyBDDfy(scenarioTitle, storyCategory).Run();
        }

        public static Engine LazyBDDfy(this object testObject, string scenarioTitle = null, string storyCategory = null)
        {
            return InternalLazyBDDfy(testObject, scenarioTitle, storyCategory);
        }

        /// <summary>
        /// Extension method to BDDfy an object instance.
        /// </summary>
        /// <typeparam name="TStory">The type representing the story.</typeparam>
        /// <param name="testObject">The test object representing a scenario.</param>
        /// <returns></returns>
        public static Story BDDfy<TStory>(this object testObject)
            where TStory : class
        {
            return BDDfy<TStory>(testObject, null);
        }

        /// <summary>
        /// Extension method to BDDfy an object instance.
        /// </summary>
        /// <typeparam name="TStory">The type representing the story.</typeparam>
        /// <param name="testObject">The test object representing a scenario.</param>
        /// <param name="scenarioTitle">Overrides the default scenario title and is displayed in the reports.</param>
        /// <returns></returns>
        public static Story BDDfy<TStory>(this object testObject, string scenarioTitle)
            where TStory : class
        {
            return testObject.LazyBDDfy<TStory>(scenarioTitle).Run();
        }

        /// <summary>
        /// Extension method to BDDfy an object instance.
        /// </summary>
        /// <typeparam name="TStory">The type representing the story.</typeparam>
        /// <param name="testObject">The test object representing a scenario.</param>
        /// <param name="scenarioTitle">Overrides the default scenario title and is displayed in the reports.</param>
        /// <param name="storyCategory">Used for filename in Html reports. Has no effect on console reports.</param>
        /// <returns></returns>
        public static Story BDDfy<TStory>(this object testObject, string scenarioTitle, string storyCategory)
            where TStory : class
        {
            return testObject.LazyBDDfy<TStory>(scenarioTitle, storyCategory).Run();
        }

        public static Engine LazyBDDfy<TStory>(this object testObject, string scenarioTitle = null, string storyCategory = null)
            where TStory : class
        {
            return InternalLazyBDDfy(testObject, scenarioTitle, storyCategory, typeof(TStory));
        }

        static Engine InternalLazyBDDfy(
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