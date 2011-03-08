using System;
using System.Collections.Generic;
using System.Reflection;

namespace Bddify
{
    public interface IScanner
    {
        IEnumerable<MethodInfo> Scan(Type type);
    }
}