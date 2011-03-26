using System;
using System.Collections.Generic;
using System.Linq;
using Bddify.Core;

namespace Bddify.Scanners
{
    public abstract class DefaultScannerBase : IScanner
    {
        protected object TestObject;

        protected virtual IEnumerable<object []> GetArgsSets()
        {
            var runWithScenarioAtts = (RunScenarioWithArgsAttribute[])TestObject.GetType().GetCustomAttributes(typeof(RunScenarioWithArgsAttribute), false);

            return runWithScenarioAtts.Select(argSet => argSet.ScenarioArguments).ToList();
        }

        string ScenarioText
        {
            get
            {
                return NetToString.FromTypeName(TestObject.GetType().Name);
            }
        }

        protected virtual Scenario GetScenario(bool instantiateNewObject, object[] argsSet = null)
        {
            var scenarioText = ScenarioText;
            if (argsSet != null)
                scenarioText += string.Format(" with args ({0})", string.Join(", ", argsSet));

            // Instantiating a new object per scenario so that scenarios in RunScenarioWithArgs run in isolation.
            object testObject = TestObject;
            if (instantiateNewObject)
                testObject = Activator.CreateInstance(TestObject.GetType());

            return new Scenario(testObject, ScanForSteps(), scenarioText, argsSet);
        }

        abstract protected IEnumerable<ExecutionStep> ScanForSteps();

        public virtual IEnumerable<Scenario> Scan(object testObject)
        {
            TestObject = testObject;
            var argsSet = GetArgsSets();
            if(argsSet.Any())
                return argsSet.Select(a => GetScenario(true, a));

            return new[] { GetScenario(false) };
        }
    }
}