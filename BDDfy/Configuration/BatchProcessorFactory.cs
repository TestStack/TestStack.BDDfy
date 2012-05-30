using System;
using System.Collections.Generic;
using BDDfy.Core;

namespace BDDfy.Configuration
{
    public class BatchProcessorFactory : ComponentFactory<IBatchProcessor, IEnumerable<Story>>
    {
        internal BatchProcessorFactory(Func<IBatchProcessor> factory) : base(factory)
        {
        }

        internal BatchProcessorFactory(Func<IBatchProcessor> factory, bool active) : base(factory, active)
        {
        }
    }
}