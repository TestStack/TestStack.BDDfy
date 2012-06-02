using System.Collections.Generic;

namespace TestStack.BDDfy.Core
{
    public interface IBatchProcessor
    {
        void Process(IEnumerable<Story> stories);
    }
}