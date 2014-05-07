using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TestStack.BDDfy.Configuration;

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

        public virtual IEnumerable<Scenario> Scan(ITestContext testContext)
        {
            Type scenarioType;
            string scenarioTitle;

            if (testContext.Examples == null)
            {
                var steps = ScanScenarioForSteps(testContext);
                scenarioType = testContext.TestObject.GetType();
                scenarioTitle = _scenarioTitle ?? GetScenarioText(scenarioType);

                var orderedSteps = steps.OrderBy(o => o.ExecutionOrder).ThenBy(o => o.ExecutionSubOrder).ToList();
                yield return new Scenario(testContext.TestObject, orderedSteps, scenarioTitle, testContext.Tags);
                yield break;
            }

            scenarioType = testContext.TestObject.GetType();
            scenarioTitle = _scenarioTitle ?? GetScenarioText(scenarioType);

            var scenarioId = Configurator.IdGenerator.GetScenarioId();

            foreach (var example in testContext.Examples)
            {
                var steps = ScanScenarioForSteps(testContext, example);
                var orderedSteps = steps.OrderBy(o => o.ExecutionOrder).ThenBy(o => o.ExecutionSubOrder).ToList();
                yield return new Scenario(scenarioId, testContext.TestObject, orderedSteps, scenarioTitle, example, new List<StepArgument>(), testContext.Tags);
            }
        }

        static string GetScenarioText(Type scenarioType)
        {
            return Configurator.Scanners.Humanize(scenarioType.Name);
        }

        protected virtual IEnumerable<Step> ScanScenarioForSteps(ITestContext testContext)
        {
            var allSteps = new List<Step>();
            var scenarioType = testContext.TestObject.GetType();

            foreach (var methodInfo in GetMethodsOfInterest(scenarioType))
            {
                // chain of responsibility of step scanners
                foreach (var scanner in _stepScanners)
                {
                    var steps = scanner.Scan(testContext, methodInfo);
                    if (steps.Any())
                    {
                        allSteps.AddRange(steps);
                        break;
                    }
                }
            }

            return allSteps;
        }

        protected virtual IEnumerable<Step> ScanScenarioForSteps(ITestContext testContext, Example example)
        {
            var allSteps = new List<Step>();
            var scenarioType = testContext.TestObject.GetType();

            foreach (var methodInfo in GetMethodsOfInterest(scenarioType))
            {
                // chain of responsibility of step scanners
                foreach (var scanner in _stepScanners)
                {
                    var steps = scanner.Scan(testContext, methodInfo, example);
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