using System;
using Bddify.Core;
using Bddify.Processors;
using Bddify.Reporters;
using Bddify.Scanners;
using System.Collections.Generic;
using Bddify.Scanners.ScenarioScanners;
using Bddify.Scanners.StepScanners.ExecutableAttribute;
using Bddify.Scanners.StepScanners.MethodName;

namespace Bddify
{
    public static class BddifyExtensions
    {
        static IScanner GetDefaultScanner(object testObject, string scenarioTitle = null)
        {
            var reflectiveScenarioScanner = GetReflectiveScenarioScanner(scenarioTitle, testObject);

            return  new DefaultScanner(testObject, reflectiveScenarioScanner);
        }

        static IScanner GetDefaultScanner<TStory>(object testObject, string scenarioTitle = null)
            where TStory : class
        {
            var reflectiveScenarioScanner = GetReflectiveScenarioScanner(scenarioTitle, testObject);

            return  new DefaultScanner<TStory>(testObject, reflectiveScenarioScanner);
        }

        private static ReflectiveScenarioScanner GetReflectiveScenarioScanner(string scenarioTitle, object testObject)
        {
            return new ReflectiveScenarioScanner(
                scenarioTitle,
                new ExecutableAttributeStepScanner(),
                new DefaultMethodNameStepScanner(testObject));
        }

        public static Story Bddify(this object testObject)
        {
            return Bddify(testObject, null);
        }

        public static Story Bddify(this object testObject, string scenarioTitle)
        {
            return testObject.LazyBddify(scenarioTitle).Run();
        }

        public static Story Bddify(this object testObject, string scenarioTitle = null, string reportCategory = null)
        {
            return testObject.LazyBddify(scenarioTitle, reportCategory).Run();
        }

        public static Bddifier LazyBddify(this object testObject, string scenarioTitle = null, string reportCategory = null)
        {
            return InternalLazyBddify(testObject, scenarioTitle, reportCategory, GetDefaultScanner);
        }

        public static Story Bddify<TStory>(this object testObject)
            where TStory : class
        {
            return Bddify<TStory>(testObject, null);
        }

        public static Story Bddify<TStory>(this object testObject, string scenarioTitle)
            where TStory : class
        {
            return testObject.LazyBddify<TStory>(scenarioTitle).Run();
        }

        public static Story Bddify<TStory>(this object testObject, string scenarioTitle = null, string reportCategory = null)
            where TStory : class
        {
            return testObject.LazyBddify<TStory>(scenarioTitle, reportCategory).Run();
        }

        public static Bddifier LazyBddify<TStory>(this object testObject, string scenarioTitle = null, string reportCategory = null)
            where TStory : class
        {
            return InternalLazyBddify(testObject, scenarioTitle, reportCategory, GetDefaultScanner<TStory>);
        }

        static Bddifier InternalLazyBddify(
            object testObject, 
            string scenarioTitle, 
            string reportCategory, 
            Func<object, string, IScanner> getDefaultScanner)
        {
            IScanner scanner = null;
            var hasScanner = testObject as IHasScanner;

            if (hasScanner != null)
            {
                scanner = hasScanner.GetScanner(scenarioTitle);
                testObject = hasScanner.TestObject;
            }

            var processors = new List<IProcessor>
                                 {
                                     new TestRunner(),
                                     new StoryReporter(reportCategory),
                                     new ExceptionProcessor()
                                 };

            var storyScanner = scanner ?? getDefaultScanner(testObject, scenarioTitle);

            return new Bddifier(storyScanner, processors);
        }
    }
}