using System;
using System.Collections.Generic;
using System.Linq;
using Bddify.Core;

namespace Bddify.Scanners
{
    public abstract class DefaultScannerBase : IScanner
    {
        private static readonly Func<IEnumerable<Type>> StoryScenariosQuery = () =>
            from assembly in AppDomain.CurrentDomain.GetAssemblies()
            from type in assembly.GetTypes()
            where Attribute.IsDefined(type, typeof(WithStoryAttribute), true)
            select type;
        private Lazy<IEnumerable<Type>> _storyScenarios = new Lazy<IEnumerable<Type>>(StoryScenariosQuery);

        protected virtual IEnumerable<object []> GetArgsSets(object scenarioObject)
        {
            var runWithScenarioAtts = (RunScenarioWithArgsAttribute[])scenarioObject.GetType().GetCustomAttributes(typeof(RunScenarioWithArgsAttribute), false);

            return runWithScenarioAtts.Select(argSet => argSet.ScenarioArguments).ToList();
        }

        static string GetScenarioText(object scenarioObject)
        {
            return NetToString.FromTypeName(scenarioObject.GetType().Name);
        }

        protected virtual Scenario GetScenario(object scenarioObject, bool instantiateNewObject = false, object[] argsSet = null)
        {
            var scenarioText = GetScenarioText(scenarioObject);
            if (argsSet != null)
                scenarioText += string.Format(" with args ({0})", string.Join(", ", argsSet));

            // Instantiating a new object per scenario so that scenarios in RunScenarioWithArgs run in isolation.
            object testObject = scenarioObject;
            if (instantiateNewObject)
                testObject = Activator.CreateInstance(testObject.GetType());

            return new Scenario(testObject, ScanForSteps(testObject), scenarioText, argsSet);
        }

        abstract protected IEnumerable<ExecutionStep> ScanForSteps(object scenarioObject);

        public virtual IEnumerable<Scenario> Scan(object testObject)
        {
            return IsStory(testObject) ? HandleStory(testObject) : HandleScenario(testObject);
        }

        private IEnumerable<Scenario> HandleScenario(object scenarioObject)
        {
            var argsSet = GetArgsSets(scenarioObject);
            if(argsSet.Any())
                return argsSet.Select(a => GetScenario(scenarioObject, instantiateNewObject:true, argsSet:a));

            return new[] { GetScenario(scenarioObject) };
        }

        private IEnumerable<Scenario> HandleStory(object storyObject)
        {
            var scenarioTypesForThisStory = _storyScenarios.Value.Where(t => t
                .GetCustomAttributes(typeof(WithStoryAttribute), true)
                .Cast<WithStoryAttribute>()
                .Any(a => a.StoryType == storyObject.GetType()));

            foreach (var scenarioType in scenarioTypesForThisStory)
            {
                // ToDo: I may change this to use IoC
                var scenarioObject = Activator.CreateInstance(scenarioType);

                foreach (var scenario in HandleScenario(scenarioObject))
                    yield return scenario;
            }
            yield break; 
        }

        internal bool IsStory(object testObject)
        {
            return testObject.GetType().GetCustomAttributes(typeof(StoryAttribute), false).Any();
        }
    }
}