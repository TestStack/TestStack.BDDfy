using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;

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
            Scenario = _scanner.Scan(_testObject);
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            //run processors in the right order regardless of the order they are provided to the Bddifer
            foreach (var processor in _processors.OrderBy(p => (int)p.ProcessType))
                processor.Process(Scenario);
            stopWatch.Stop();
            Scenario.Duration = stopWatch.Elapsed;
        }

        public Scenario Scenario {get; private set;}
    }
}