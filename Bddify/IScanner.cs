using System;
using System.Collections.Generic;

namespace Bddify
{
    public interface IScanner
    {
        IEnumerable<ExecutionStep> Scan(Type typeToScan);
    }
}