using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bddify.Scanners.ScenarioScanners;
using Bddify.Core;
using System.Diagnostics;

namespace Bddify.Scanners.StepScanners.Fluent
{
    class FluentScenarioScanner : IScanForScenario
    {
        private string _title;
        private IEnumerable<ExecutionStep> _steps;

        public FluentScenarioScanner(IEnumerable<ExecutionStep> steps, string title = null)
        {
            _title = title;
            _steps = steps;
        }

        public Scenario Scan(object testObject)
        {
            var scenarioText = _title ?? GetTitleFromMethodNameInStackTrace(testObject);
            return new Scenario(testObject, _steps, scenarioText);
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
