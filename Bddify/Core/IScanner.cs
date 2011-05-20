using System;

namespace Bddify.Core
{
    public interface IScanner
    {
        Story Scan(Type scenarioType);
    }
}