using System.Collections.Generic;

namespace Bddify.Core
{
    public interface IScanner
    {
        IEnumerable<Scenario> Scan(object testObject);
    }
}