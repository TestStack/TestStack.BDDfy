using System.Collections.Generic;
using System.Linq;
using Bddify.Core;

namespace Bddify.Scanners
{
    public abstract class DefaultScannerBase : IScanner
    {
        protected object TestObject;

        protected virtual IEnumerable<object []> GetArgsSet()
        {
            var runWithScenarioAtts = (RunScenarioWithArgsAttribute[])TestObject.GetType().GetCustomAttributes(typeof(RunScenarioWithArgsAttribute), false);

            return runWithScenarioAtts.Select(argSet => argSet.ScenarioArguments).ToList();
        }

        string ScenarioSentence
        {
            get
            {
                return NetToString.CreateSentenceFromTypeName(TestObject.GetType().Name);
            }
        }

        protected virtual Scenario GetScenario(IEnumerable<object[]> argSets = null)
        {
            return new Scenario(TestObject, ScanForSteps(), ScenarioSentence, argSets);
        }

        abstract protected IEnumerable<ExecutionStep> ScanForSteps();

        public virtual Scenario Scan(object testObject)
        {
            TestObject = testObject;
            var argsSet = GetArgsSet();
            return GetScenario(argsSet);
        }
    }
}