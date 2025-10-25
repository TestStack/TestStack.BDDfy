using System.Collections.Generic;
using System.Linq;
using TestStack.BDDfy.Configuration;

namespace TestStack.BDDfy
{
    public class FluentScenarioScanner(List<Step> steps, string title): IScenarioScanner
    {
        private readonly string _title = title;
        private readonly List<Step> _steps = steps;

        public IEnumerable<Scenario> Scan(ITestContext testContext)
        {
            var scenarioText = _title ?? GetTitleFromMethodNameInStackTrace(testContext.TestObject);
            if (testContext.Examples != null)
            {
                var scenarioId = Configurator.IdGenerator.GetScenarioId();
                return testContext.Examples.Select(example =>
                    new Scenario(scenarioId, testContext.TestObject, CloneSteps(_steps), scenarioText, example, testContext.Tags));
            }

            return new[] { new Scenario(testContext.TestObject, _steps, scenarioText, testContext.Tags) };
        }

        private List<Step> CloneSteps(IEnumerable<Step> steps)
        {
            return steps.Select(step => new Step(step)).ToList();
        }

        private static string GetTitleFromMethodNameInStackTrace(object testObject)
        {
            // ReSharper disable once JoinDeclarationAndInitializer
#if STACKTRACE
            var trace = new System.Diagnostics.StackTrace();
            var frames = trace.GetFrames();

            var initiatingFrame = frames?.LastOrDefault(s => s.GetMethod().DeclaringType == testObject.GetType());
            if (initiatingFrame == null)
                return null;

            return Configurator.Humanizer.Humanize(initiatingFrame.GetMethod().Name);
#else
            return null;
#endif
        }
    }
}
