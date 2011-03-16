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

        public Bddee Run()
        {
            var steps = _scanner.Scan(_testObject.GetType());
            Bddee = new Bddee(_testObject, steps);

            //run processors in the right order regardless of the order they are provided to the Bddifer
            foreach (var processor in _processors.OrderBy(p => (int)p.ProcessType))
                processor.Process(Bddee);

            return Bddee;
        }

        public Bddee Bddee { get; private set; }
    }
}