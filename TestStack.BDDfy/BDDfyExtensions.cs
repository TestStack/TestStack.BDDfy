using System;
using System.Linq;
using TestStack.BDDfy.Configuration;

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

        public static Story BDDfy(this IExamples examples)
        {
            return examples.TestObject.BDDfy();
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