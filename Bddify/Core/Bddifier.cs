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

            var bddee = GetBddee();
            RunBddee(bddee);
        }

        private void RunBddee(Bddee bddee)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            //run processors in the right order regardless of the order they are provided to the Bddifer
            foreach (var processor in _processors.OrderBy(p => (int)p.ProcessType))
                processor.Process(bddee);
            stopWatch.Stop();
            bddee.Duration = stopWatch.Elapsed;
        }

        string ScenarioSentence
        {
            get
            {
                return NetToString.CreateSentenceFromTypeName(_testObject.GetType().Name);
            }
        }

        private Bddee GetBddee(string scenarioSentence = null)
        {
            if (string.IsNullOrEmpty(scenarioSentence))
                scenarioSentence = ScenarioSentence;

            var steps = _scanner.Scan(_testObject.GetType());
            var bddee = new Bddee(_testObject, steps, scenarioSentence);
            _bddees.Add(bddee);
            return bddee;
        }

        bool RunScenarioWith()
        {
            var runWithScenarioAtts = (RunScenarioWithArgsAttribute[])_testObject.GetType().GetCustomAttributes(typeof(RunScenarioWithArgsAttribute), false);
            if(!runWithScenarioAtts.Any())
                return false;

            foreach (var argSet in runWithScenarioAtts)
            {
                var bddee = GetBddee(ScenarioSentence + " with args (" + string.Join(", ", argSet.ScenarioArguments) + ")");
                var argSetterMethod = _testObject.GetType().GetMethod("RunScenarioWithArgs", BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                argSetterMethod.Invoke(_testObject, argSet.ScenarioArguments);
                RunBddee(bddee);
            }
            return true;
        }

        private List<Bddee> _bddees = new List<Bddee>();
        public IEnumerable<Bddee> Bddees
        {
            get { return _bddees; }
        }
    }
}