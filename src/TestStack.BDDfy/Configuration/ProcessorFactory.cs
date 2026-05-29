using System;

namespace TestStack.BDDfy.Configuration
{
    public class ProcessorFactory : ComponentFactory<IProcessor, Story>
    {
        public ProcessorFactory(Func<IProcessor> factory) : base(factory)
        {
        }

        public ProcessorFactory(Func<IProcessor> factory, bool active) : base(factory, active)
        {
        }
    }
}