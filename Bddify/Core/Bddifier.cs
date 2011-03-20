using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace Bddify.Core
{
    public class Bddifier
    {
        private readonly IEnumerable<IProcessor> _processors;
        private readonly object _testObject;
        private readonly IScanner _scanner;

        public Bddifier(object testObject, IScanner scanner, IEnumerable<IProcessor> processors)
        {
            _processors = processors;
            _testObject = testObject;
            _scanner = scanner;
        }

        public void Run()
        {
            if (RunScenarioWith())
                return;

            var scenario = GetScenario();
            Run(scenario);
        }

        private void Run(Scenario scenario)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            //run processors in the right order regardless of the order they are provided to the Bddifer
            foreach (var processor in _processors.OrderBy(p => (int)p.ProcessType))
                processor.Process(scenario);
            stopWatch.Stop();
            scenario.Duration = stopWatch.Elapsed;
        }

        string ScenarioSentence
        {
            get
            {
                return NetToString.CreateSentenceFromTypeName(_testObject.GetType().Name);
            }
        }

        private Scenario GetScenario(string scenarioSentence = null)
        {
            if (string.IsNullOrEmpty(scenarioSentence))
                scenarioSentence = ScenarioSentence;

            var steps = _scanner.Scan(_testObject.GetType());
            var scenario = new Scenario(_testObject, steps, scenarioSentence);
            _scenarios.Add(scenario);
            return scenario;
        }

        bool RunScenarioWith()
        {
            var runWithScenarioAtts = (RunScenarioWithArgsAttribute[])_testObject.GetType().GetCustomAttributes(typeof(RunScenarioWithArgsAttribute), false);
            if(!runWithScenarioAtts.Any())
                return false;

            foreach (var argSet in runWithScenarioAtts)
            {
                var scenario = GetScenario(ScenarioSentence + " with args (" + string.Join(", ", argSet.ScenarioArguments) + ")");
                var argSetterMethod = _testObject.GetType().GetMethod("RunScenarioWithArgs", BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                argSetterMethod.Invoke(_testObject, argSet.ScenarioArguments);
                Run(scenario);
            }
            return true;
        }

        private readonly List<Scenario> _scenarios = new List<Scenario>();
        public IEnumerable<Scenario> Scenarios
        {
            get { return _scenarios; }
        }
    }
}