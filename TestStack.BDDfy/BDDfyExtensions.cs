using System;
using System.Linq;
using TestStack.BDDfy.Configuration;

namespace TestStack.BDDfy
{
    public static class BDDfyExtensions
    {
        /// <summary>
        /// Extension method to BDDfy an object instance.
        /// </summary>
        /// <param name="testObject">The test object representing a scenario.</param>
        /// <param name="scenarioTitle">Overrides the default scenario title and is displayed in the reports.</param>
        /// <param name="storyCategory">Used for filename in Html reports. Has no effect on console reports.</param>
        /// <returns></returns>
        public static Story BDDfy(this object testObject, string scenarioTitle = null, string storyCategory = null)
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
        /// <param name="scenarioTitle">Overrides the default scenario title and is displayed in the reports.</param>
        /// <param name="storyCategory">Used for filename in Html reports. Has no effect on console reports.</param>
        /// <returns></returns>
        public static Story BDDfy<TStory>(this object testObject, string scenarioTitle = null, string storyCategory = null)
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
            var storyScanner = GetFluentScanner(testObject, scenarioTitle, explicitStoryType) ?? GetReflectiveScanner(testObject, scenarioTitle, explicitStoryType);

            return new Engine(storyCategory, storyScanner);
        }

        static IScanner GetReflectiveScanner(object testObject, string scenarioTitle = null, Type explicitStoryType = null)
        {
            var stepScanners = Configurator.Scanners.GetStepScanners(testObject).ToArray();
            var reflectiveScenarioScanner = new ReflectiveScenarioScanner(scenarioTitle, stepScanners);

            return new DefaultScanner(testObject, reflectiveScenarioScanner, explicitStoryType);
        }

        static IScanner GetFluentScanner(object testObject, string scenarioTitle, Type explicitStoryType)
        {
            IScanner scanner = null;

            var examples = testObject as IExamples;

            var fluentScanner = testObject as IFluentScanner;
            if (examples != null)
            {
                fluentScanner = examples.TestObject as IFluentScanner;
            }

            if (fluentScanner != null)
            {
                scanner = fluentScanner.GetScanner(scenarioTitle, explicitStoryType, examples);
            }

            return scanner;
        }
    }
}