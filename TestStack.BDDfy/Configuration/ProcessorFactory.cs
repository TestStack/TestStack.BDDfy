using System;
using TestStack.BDDfy.Core;

namespace TestStack.BDDfy.Configuration
{
    public class ProcessorFactory : ComponentFactory<IProcessor, Story>
    {
        internal ProcessorFactory(Func<IProcessor> factory) : base(factory)
        {
        }

        internal ProcessorFactory(Func<IProcessor> factory, bool active) : base(factory, active)
        {
        }
    }
}