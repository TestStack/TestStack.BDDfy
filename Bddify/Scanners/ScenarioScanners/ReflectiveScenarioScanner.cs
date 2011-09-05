using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Bddify.Core;
using Bddify.Scanners.StepScanners;
using System.Reflection;

namespace Bddify.Scanners.ScenarioScanners
{
    public class ReflectiveScenarioScanner : IScenarioScanner
    {
        private readonly IEnumerable<IStepScanner> _stepScanners;
        private readonly string _scenarioTitle;

        public ReflectiveScenarioScanner(params IStepScanner[] stepScanners)
            : this(null, stepScanners)
        {
        }

        public ReflectiveScenarioScanner(string scenarioTitle, params IStepScanner[] stepScanners)
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