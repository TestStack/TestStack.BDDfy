using System;
using System.Collections.Generic;
using System.Linq;
using Bddify.Core;
using Bddify.Scanners.StepScanners;

namespace Bddify.Scanners.ScenarioScanners
{
    public class ScanForScenarios : IScanForScenarios
    {
        private readonly IEnumerable<IScanForSteps> _stepScanners;
        private readonly string _scenarioTitle;

        public ScanForScenarios(IEnumerable<IScanForSteps> stepScanners)
            : this(stepScanners, null)
        {
        }

        public ScanForScenarios(IEnumerable<IScanForSteps> stepScanners, string scenarioTitle)
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
            var steps = new List<ExecutionStep>();
            // scanners are sorted by priority to make sure the higher priority scanners get to scan the scenario first
            foreach (var scanner in _stepScanners.OrderBy(s => s.Priority))
            {
                foreach (var step in scanner.Scan(testObject))
                {
                    // if a method has been found by another scanner, ignore it
                    if (steps.Any(s => s.Equals(step))) 
                        continue;

                    steps.Add(step);
                    yield return step;
                }
            }
        }
    }
}