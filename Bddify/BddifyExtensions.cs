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

        public static Story Bddify(this object testObject, string scenarioTitle = null, string storyCategory = null)
        {
            return testObject.LazyBddify(scenarioTitle, storyCategory).Run();
        }

        public static Bddifier LazyBddify(this object testObject, string scenarioTitle = null, string storyCategory = null)
        {
            return InternalLazyBddify(testObject, scenarioTitle, storyCategory, GetDefaultScanner);
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

        public static Story Bddify<TStory>(this object testObject, string scenarioTitle = null, string storyCategory = null)
            where TStory : class
        {
            return testObject.LazyBddify<TStory>(scenarioTitle, storyCategory).Run();
        }

        public static Bddifier LazyBddify<TStory>(this object testObject, string scenarioTitle = null, string storyCategory = null)
            where TStory : class
        {
            return InternalLazyBddify(testObject, scenarioTitle, storyCategory, GetDefaultScanner<TStory>);
        }

        static Bddifier InternalLazyBddify(
            object testObject, 
            string scenarioTitle, 
            string storyCategory, 
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
                                     new StoryReporter(),
                                     new ExceptionProcessor()
                                 };

            var storyScanner = scanner ?? getDefaultScanner(testObject, scenarioTitle);

            return new Bddifier(storyCategory, storyScanner, processors);
        }
    }
}