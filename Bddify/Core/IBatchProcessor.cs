using System.Collections.Generic;

namespace Bddify.Core
{
    public interface IBatchProcessor
    {
        void Process(IEnumerable<Story> stories);
    }
}