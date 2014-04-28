using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using TestStack.BDDfy.Configuration;

namespace TestStack.BDDfy
{
    public class FluentScenarioScanner : IScenarioScanner
    {
        private readonly string _title;
        private readonly IEnumerable<Step> _steps;

        public FluentScenarioScanner(IEnumerable<Step> steps, string title)
        {
            _title = title;
            _steps = steps;
        }

        public IEnumerable<Scenario> Scan(ITestContext testContext)
        {
            var scenarioText = _title ?? GetTitleFromMethodNameInStackTrace(testContext.TestObject);
            if (testContext.Examples != null)
            {
                var scenarioId = Configurator.IdGenerator.GetScenarioId();
                return testContext.Examples.Select(example =>
                    new Scenario(scenarioId, testContext.TestObject, CloneSteps(_steps), scenarioText, example));
            }

            return new[] { new Scenario(testContext.TestObject, _steps, scenarioText) };
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

            return Configurator.Scanners.Humanize(initiatingFrame.GetMethod().Name);
        }
    }
}
