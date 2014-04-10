using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Reflection;
using TestStack.BDDfy.Configuration;

namespace TestStack.BDDfy
{
    public class FluentScenarioScanner : IScenarioScanner
    {
        private readonly string _title;
        private readonly IExampleTable _examples;
        private readonly IEnumerable<Step> _steps;

        public FluentScenarioScanner(IEnumerable<Step> steps, string title, IExampleTable examples)
        {
            _title = title;
            _examples = examples;
            _steps = steps;
        }

        public IEnumerable<Scenario> Scan(object testObject)
        {
            var scenarioText = _title ?? GetTitleFromMethodNameInStackTrace(testObject);
            if (_examples != null)
            {
                var scenarioId = Configurator.IdGenerator.GetScenarioId();
                return _examples.Select(example =>
                    new Scenario(scenarioId, testObject, CloneSteps(_steps), scenarioText, example));
            }

            return new[]{ new Scenario(testObject, _steps, scenarioText)};
        }

        private IEnumerable<Step> CloneSteps(IEnumerable<Step> steps)
        {
            return steps.Select(step => new Step(step));
        }

        private static string GetTitleFromMethodNameInStackTrace(object testObject)
        {
            var trace = new StackTrace();
            var frames = trace.GetFrames();
            if (frames == null)
                return null;

            var initiatingFrame = frames.LastOrDefault(s => s.GetMethod().DeclaringType == testObject.GetType());
            if (initiatingFrame == null)
                return null;

            return NetToString.Convert(initiatingFrame.GetMethod().Name);
        }
    }
}
