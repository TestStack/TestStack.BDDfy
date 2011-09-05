using System.Linq;
using System.Collections.Generic;
using Bddify.Processors;

namespace Bddify.Core
{
    public class Bddifier
    {
        private readonly IEnumerable<IProcessor> _processors;
        private readonly IScanner _scanner;

        public Bddifier(IScanner scanner, IEnumerable<IProcessor> processors)
        {
            _processors = processors;
            _scanner = scanner;
        }

        public Story Run()
        {
            _story = _scanner.Scan();

            try
            {
                //run processors in the right order regardless of the order they are provided to the Bddifer
                foreach (var processor in _processors.OrderBy(p => (int)p.ProcessType))
                    processor.Process(_story);
            }
            finally
            {
                var disposer = new Disposer();
                disposer.Process(_story);
            }

            return _story;
        }

        private Story _story;

        public Story Story
        {
            get { return _story; }
        }
    }
}