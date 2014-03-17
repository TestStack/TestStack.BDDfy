using System.Collections.Generic;

namespace TestStack.BDDfy
{
    public interface IBatchProcessor
    {
        void Process(IEnumerable<Story> stories);
    }
}