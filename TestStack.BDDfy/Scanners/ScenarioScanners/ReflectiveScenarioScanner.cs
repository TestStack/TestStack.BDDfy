﻿using System;
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

        public virtual IEnumerable<Scenario> Scan(object testObject)
        {
            var examples = testObject as ExampleTable;
            Type scenarioType;
            string scenarioTitle;

            if (examples == null)
            {
                var steps = ScanScenarioForSteps(testObject);
                scenarioType = testObject.GetType();
                scenarioTitle = _scenarioTitle ?? GetScenarioText(scenarioType);

                yield return new Scenario(testObject, steps, scenarioTitle);
                yield break;
            }

            testObject = examples.TestObject;
            scenarioType = testObject.GetType();
            scenarioTitle = _scenarioTitle ?? GetScenarioText(scenarioType);

            var scenarioId = Configurator.IdGenerator.GetScenarioId();

            foreach (var example in examples)
            {
                var steps = ScanScenarioForSteps(testObject, example);
                yield return new Scenario(scenarioId, testObject, steps, scenarioTitle, example);
            }
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

        protected virtual IEnumerable<Step> ScanScenarioForSteps(object testObject, Example example)
        {
            var allSteps = new List<Step>();
            var scenarioType = testObject.GetType();

            foreach (var methodInfo in GetMethodsOfInterest(scenarioType))
            {
                // chain of responsibility of step scanners
                foreach (var scanner in _stepScanners)
                {
                    var steps = scanner.Scan(testObject, methodInfo, example);
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