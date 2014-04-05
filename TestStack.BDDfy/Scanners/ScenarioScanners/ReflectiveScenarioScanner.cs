using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TestStack.BDDfy
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

        public virtual Scenario Scan(object testObject)
        {
            var examples = testObject as IExamples;
            object[][] exampleRows = null;
            if (examples != null)
            {
                testObject = examples.TestObject;
                exampleRows = examples.ExampleRows;
            }

            var scenarioType = testObject.GetType();
            var scenarioTitle = _scenarioTitle ?? GetScenarioText(scenarioType);
            var steps = ScanScenarioForSteps(testObject);

            return new Scenario(testObject, steps, scenarioTitle, exampleRows);
        }

        static string GetScenarioText(Type scenarioType)
        {
            return NetToString.Convert(scenarioType.Name);
        }

        protected virtual IEnumerable<Step> ScanScenarioForSteps(object testObject)
        {
            var allSteps = new List<Step>();
            var scenarioType = testObject.GetType();

            foreach (var methodInfo in GetMethodsOfInterest(scenarioType))
            {
                // chain of responsibility of step scanners
                foreach (var scanner in _stepScanners)
                {
                    var steps = scanner.Scan(testObject, methodInfo);
                    if (steps.Any())
                    {
                        allSteps.AddRange(steps);
                        break;
                    }
                }
            }

            return allSteps;
        }

        public virtual IEnumerable<MethodInfo> GetMethodsOfInterest(Type scenarioType)
        {
            var bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            var properties = scenarioType.GetProperties(bindingFlags);
            var getMethods = properties.Select(p => p.GetGetMethod(true));
            var setMethods = properties.Select(p => p.GetSetMethod(true));
            var allPropertyMethods = getMethods.Union(setMethods);

            return scenarioType
                .GetMethods(bindingFlags)
                .Where(m => !m.GetCustomAttributes(typeof(IgnoreStepAttribute), false).Any()) // it is not decorated with IgnoreStep
                .Except(allPropertyMethods) // properties cannot be steps; only methods
                .ToList();
        }
    }
}