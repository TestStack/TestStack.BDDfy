using System;
using System.Collections.Generic;
using System.Linq;
using Bddify.Core;

namespace Bddify.Scanners
{
    public class ScanForScenarios : IScanForScenarios
    {
        private readonly IScanForSteps _stepScanner;
        static IEnumerable<Type> _allScenarios;

        public ScanForScenarios(IScanForSteps stepScanner)
        {
            _stepScanner = stepScanner;
        }

        public IEnumerable<Scenario> Scan(Type storyType)
        {
            var storyAttribute = GetStoryAttribute(storyType);

            if(storyAttribute == null)
                return HandleScenario(storyType);
            
            var scenarios = new List<Scenario>();
            var scenarioTypesForThisStory = GetScenarioTypes(storyType);
            foreach (var scenarioType in scenarioTypesForThisStory)
                scenarios.AddRange(HandleScenario(scenarioType));

            return scenarios;
        }

        private IEnumerable<Scenario> HandleScenario(Type scenarioType)
        {
            var argsSet = GetArgsSets(scenarioType);
            if (argsSet.Any())
                return argsSet.Select(a => GetScenario(scenarioType, argsSet: a));

            return new[] { GetScenario(scenarioType) };
        }

        protected virtual IEnumerable<object[]> GetArgsSets(Type scenarioType)
        {
            var runWithScenarioAtts = (RunScenarioWithArgsAttribute[])scenarioType.GetCustomAttributes(typeof(RunScenarioWithArgsAttribute), false);

            return runWithScenarioAtts.Select(argSet => argSet.ScenarioArguments).ToList();
        }

        static string GetScenarioText(Type scenarioType)
        {
            return NetToString.FromTypeName(scenarioType.Name);
        }

        protected virtual Scenario GetScenario(Type scenarioType, object[] argsSet = null)
        {
            var scenarioText = GetScenarioText(scenarioType);
            if (argsSet != null)
                scenarioText += string.Format(" with args ({0})", string.Join(", ", argsSet));

            object testObject = Activator.CreateInstance(scenarioType);

            var steps = _stepScanner.Scan(scenarioType).ToList();

            return new Scenario(testObject, steps, scenarioText, argsSet);
        }

        private static IEnumerable<Type> GetScenarioTypes(Type storyType)
        {
            // ToDo: this is not thread-safe
            if (_allScenarios == null)
                _allScenarios = (from type in storyType.Assembly.GetTypes()
                                 where Attribute.IsDefined(type, typeof(WithStoryAttribute), true)
                                 select type).ToList();

            return _allScenarios
                .Where(t => t.GetCustomAttributes(typeof(WithStoryAttribute), true)
                                .Cast<WithStoryAttribute>()
                                .Any(a => a.StoryType == storyType));
        }

        internal StoryAttribute GetStoryAttribute(Type storyType)
        {
            return (StoryAttribute)storyType.GetCustomAttributes(typeof(StoryAttribute), false).FirstOrDefault();
        }
    }
}