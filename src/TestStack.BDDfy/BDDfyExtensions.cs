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
        /// <param name="caller">Caller (populated by [CallerMemberName])</param>
        /// <returns></returns>
        public static Story BDDfy(
            this object testObject, 
            string scenarioTitle = null, 
            [System.Runtime.CompilerServices.CallerMemberName] 
            string caller = null)
        {
            var callerName = testObject.GetActualCallerName(caller);
            return InternalLazyBDDfy(testObject, scenarioTitle ?? Configurator.Humanize(callerName)).Run();
        }

        public static Engine LazyBDDfy(
            this object testObject, 
            string scenarioTitle = null, 
            [System.Runtime.CompilerServices.CallerMemberName] 
            string caller = null)
        {
            var callerName = testObject.GetActualCallerName(caller);
            return InternalLazyBDDfy(testObject, scenarioTitle ?? Configurator.Humanize(callerName));
        }

        /// <summary>
        /// Extension method to BDDfy an object instance.
        /// </summary>
        /// <typeparam name="TStory">The type representing the story.</typeparam>
        /// <param name="testObject">The test object representing a scenario.</param>
        /// <param name="scenarioTitle">Overrides the default scenario title and is displayed in the reports.</param>
        /// <param name="caller">Caller (populated by [CallerMemberName])</param>
        /// <returns></returns>
        public static Story BDDfy<TStory>(
            this object testObject,
            string scenarioTitle = null,
            [System.Runtime.CompilerServices.CallerMemberName]
            string caller = null)
        where TStory : class
        {
            var callerName = testObject.GetActualCallerName(caller);
            return InternalLazyBDDfy(testObject, scenarioTitle ?? Configurator.Humanize(callerName), typeof(TStory)).Run();
        }

        public static Engine LazyBDDfy<TStory>(
            this object testObject,
            string scenarioTitle = null,
            [System.Runtime.CompilerServices.CallerMemberName]
            string caller = null)
        where TStory : class
        {
            var callerName = testObject.GetActualCallerName(caller);
            return InternalLazyBDDfy(testObject, scenarioTitle ?? Configurator.Humanize(callerName), typeof(TStory));
        }

        static Engine InternalLazyBDDfy(
            object testObject,
            string scenarioTitle,
            Type explicitStoryType = null)
        {
            var testContext = TestContext.GetContext(testObject);

            var storyScanner = testContext.FluentScanner != null ?
                testContext.FluentScanner.GetScanner(scenarioTitle, explicitStoryType) :
                GetReflectiveScanner(testContext, scenarioTitle, explicitStoryType);

            return new (storyScanner);
        }

        static DefaultScanner GetReflectiveScanner(ITestContext testContext, string scenarioTitle = null, Type explicitStoryType = null)
        {
            var stepScanners = Configurator.Scanners.GetStepScanners(testContext).ToArray();
            var reflectiveScenarioScanner = new ReflectiveScenarioScanner(scenarioTitle, stepScanners);

            return new (testContext, reflectiveScenarioScanner, explicitStoryType);
        }

        static string GetActualCallerName(this object testObject, string inferedCallerName)
            => inferedCallerName == ".ctor" ? testObject.GetType().Name : inferedCallerName;
    }
}