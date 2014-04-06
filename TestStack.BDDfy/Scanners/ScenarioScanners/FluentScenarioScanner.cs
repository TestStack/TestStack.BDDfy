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
                    new Scenario(scenarioId, testObject, CloneSteps(_steps), scenarioText, example)
                    {
                        Init = o => PrepareTestObject(o, example)
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

        private void PrepareTestObject(object testObject, Example example)
        {
            foreach (var column in example)
            {
                var type = testObject.GetType();
                //TODO should we throw if match both a field and a property?
                var matchingMembers = type.GetMembers()
                    .Where(m => m is FieldInfo || m is PropertyInfo)
                    .FirstOrDefault(n => n.Name.Equals(column.Key, StringComparison.InvariantCultureIgnoreCase));

                if (matchingMembers == null)
                    throw new InvalidOperationException(
                        string.Format("Expecting a fields or a property with name of {0} to match example header",
                            column.Key));

                var prop = matchingMembers as PropertyInfo;
                if (prop != null)
                    prop.SetValue(testObject, column.Value, null);

                var field = matchingMembers as FieldInfo;
                if (field != null)
                    field.SetValue(testObject, column.Value);
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
