using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using TestStack.BDDfy.Configuration;

namespace TestStack.BDDfy
{
    public class FluentScenarioScanner : IScenarioScanner
    {
        private readonly string _title;
        private readonly IExamples _examples;
        private readonly IEnumerable<Step> _steps;

        public FluentScenarioScanner(IEnumerable<Step> steps, string title, IExamples examples)
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
                var exampleHeaders = _examples.ExampleHeaders;
                var exampleRows = _examples.ExampleRows;

                var scenarioId = Configurator.IdGenerator.GetScenarioId();
                return exampleRows.Select((r, i) =>
                    new Scenario(scenarioId, testObject, _steps, scenarioText, exampleHeaders, r, i));
            }

            return new[]{ new Scenario(testObject, _steps, scenarioText)};
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
