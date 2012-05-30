using System.Collections.Generic;

namespace BDDfy.Core
{
    public interface IBatchProcessor
    {
        void Process(IEnumerable<Story> stories);
    }
}