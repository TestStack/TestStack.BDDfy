using System;
using Bddify.Core;

namespace Bddify.Configuration
{
    public class ProcessorFactory
    {
        private bool _active = true;
        private Predicate<Story> _runsOn = story => true;
        readonly Func<IProcessor> _factory; 
        internal ProcessorFactory(Func<IProcessor> factory)
        {
            _factory = factory;
        }

        public void Disable()
        {
            _active = false;
        }

        public void Enable()
        {
            _active = true;
        }

        public IProcessor ConstructFor(Story story)
        {
            if (!_active || !_runsOn(story))
                return null;
            
            return _factory();
        }

        public void RunsOn(Predicate<Story> runOn)
        {
            _runsOn = runOn;
        }
    }
}