using System;
using System.Collections.Generic;
using System.Linq;
using Bddify.Core;
using Bddify.Scanners.StepScanners;
using System.Reflection;

namespace Bddify.Scanners.ScenarioScanners
{
    public class ScanForScenario : IScanForScenario
    {
        private readonly IEnumerable<IScanForSteps> _stepScanners;
        private readonly string _scenarioTitle;

        public ScanForScenario(IEnumerable<IScanForSteps> stepScanners)
            : this(stepScanners, null)
        {
        }

        public ScanForScenario(IEnumerable<IScanForSteps> stepScanners, string scenarioTitle)
        {
            _stepScanners = stepScanners;
            _scenarioTitle = scenarioTitle;
        }

        public Scenario Scan(object testObject)
        {
            var scenarioType = testObject.GetType();
            var scenarioTitle = _scenarioTitle ?? GetScenarioText(scenarioType);
            var steps = ScanScenarioForSteps(testObject);

            return new Scenario(testObject, steps, scenarioTitle);
        }

        static string GetScenarioText(Type scenarioType)
        {
            return NetToString.Convert(scenarioType.Name);
        }

        private IEnumerable<ExecutionStep> ScanScenarioForSteps(object testObject)
        {
            var allSteps = new List<ExecutionStep>();
            var scenarioType = testObject.GetType();
            foreach (var methodInfo in GetMethodsOfInterest(scenarioType))
            {
                foreach (var scanner in _stepScanners)
                {
                    var steps = scanner.Scan(methodInfo);
                    if (steps.Any())
                    {
                        allSteps.AddRange(steps);
                        break;
                    }
                }
            }

            return allSteps;
        }

        public static IEnumerable<MethodInfo> GetMethodsOfInterest(Type scenarioType)
        {
            return scenarioType
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(m => !m.GetCustomAttributes(typeof(IgnoreStepAttribute), false).Any())
                .ToList();
        }
    }
}