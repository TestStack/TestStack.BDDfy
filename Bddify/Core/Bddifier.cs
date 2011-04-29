using System;
using System.Linq;
using System.Collections.Generic;
using Bddify.Processors;

namespace Bddify.Core
{
    public class Bddifier
    {
        private readonly IEnumerable<IProcessor> _processors;
        private readonly Type _storyType;
        private readonly IScanner _scanner;

        public Bddifier(object testObject, IScanner scanner, IEnumerable<IProcessor> processors)
        {
            _processors = processors;
            _storyType = testObject.GetType();
            _scanner = scanner;
        }

        public void Run()
        {
            _story = _scanner.Scan(_storyType);

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
        }

        private Story _story;

        public Story Story
        {
            get { return _story; }
        }
    }
}