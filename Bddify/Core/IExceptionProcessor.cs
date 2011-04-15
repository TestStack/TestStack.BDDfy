using System.Collections.Generic;

namespace Bddify.Core
{
    public interface IExceptionProcessor
    {
        void ProcessExceptions(IEnumerable<Scenario> scenarios);
    }
}