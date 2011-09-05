using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bddify.Core
{
    public interface IHasScanner
    {
        IScanner GetScanner(string scenarioTitle);
        object TestObject { get; }
    }
}
