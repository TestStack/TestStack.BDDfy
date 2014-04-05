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
                    new Scenario(scenarioId, testObject, CloneSteps(_steps), scenarioText, exampleHeaders, r, i)
                    {
                        Init = o=> PrepareTestObject(o, exampleHeaders, r, i)
                    });
            }

            return new[]{ new Scenario(testObject, _steps, scenarioText)};
        }

        private IEnumerable<Step> CloneSteps(IEnumerable<Step> steps)
        {
            return steps.Select(step => new Step(step)
            {
            });
        }

        private void PrepareTestObject(object testObject, string[] exampleHeaders, object[] examples, int rowIndex)
        {
            for (int index = 0; index < exampleHeaders.Length; index++)
            {
                var exampleHeader = exampleHeaders[index];
                var exampleValue = examples[index];
                var type = testObject.GetType();
                //TODO should we throw if match both a field and a property?
                var matchingMembers = type.GetMembers()
                    .Where(m => m is FieldInfo || m is PropertyInfo)
                    .FirstOrDefault(n => n.Name.Equals(exampleHeader, StringComparison.InvariantCultureIgnoreCase));

                if (matchingMembers == null)
                    throw new InvalidOperationException(
                        string.Format("Expecting a fields or a property with name of {0} to match example header",
                            exampleHeader));

                var prop = matchingMembers as PropertyInfo;
                if (prop != null)
                    prop.SetValue(testObject, exampleValue, null);

                var field = matchingMembers as FieldInfo;
                if (field != null)
                    field.SetValue(testObject, exampleValue);
            }
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
