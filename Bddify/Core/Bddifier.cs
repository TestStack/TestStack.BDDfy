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
        private readonly IExceptionProcessor _exceptionProcessor;

        public Bddifier(object testObject, IScanner scanner, IExceptionProcessor exceptionProcessor, IEnumerable<IProcessor> processors)
        {
            _processors = processors;
            _testObject = testObject;
            _scanner = scanner;
            _exceptionProcessor = exceptionProcessor;
        }

        public void Run()
        {
            foreach (var scenario in _scanner.Scan(_testObject))
            {
                _scenarios.Add(scenario);
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                //run processors in the right order regardless of the order they are provided to the Bddifer
                foreach (var processor in _processors.OrderBy(p => (int)p.ProcessType))
                    processor.Process(scenario);
                stopWatch.Stop();
                scenario.Duration = stopWatch.Elapsed;
            }

            if(_exceptionProcessor != null)
                _exceptionProcessor.ProcessExceptions(Scenarios);
        }

        private readonly List<Scenario> _scenarios = new List<Scenario>();

        public IEnumerable<Scenario> Scenarios
        {
            get { return _scenarios; }
        }
    }
}