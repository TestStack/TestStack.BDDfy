using System;
using System.Collections.Generic;

namespace Bddify.Core
{
    public interface IScanner
    {
        IEnumerable<ExecutionStep> Scan(Type typeToScan);
    }
}