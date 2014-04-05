using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace TestStack.BDDfy
{
    public class FluentScenarioScanner : IScenarioScanner
    {
        private readonly string _title;
        private readonly IEnumerable<Step> _steps;

        public FluentScenarioScanner(IEnumerable<Step> steps, string title = null)
        {
            _title = title;
            _steps = steps;
        }

        public Scenario Scan(object testObject)
        {
            object[][] exampleRows = null;
            var examples = testObject as IExamples;

            if (examples != null)
            {
                testObject = examples.TestObject;
                exampleRows = examples.ExampleRows;
            }

            var scenarioText = _title ?? GetTitleFromMethodNameInStackTrace(testObject);
            return new Scenario(testObject, _steps, scenarioText, exampleRows);
        }

        internal static string GetTitleFromMethodNameInStackTrace(object testObject)
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
