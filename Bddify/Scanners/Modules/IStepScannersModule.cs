using System.Collections.Generic;
using Bddify.Module;
using Bddify.Scanners.StepScanners;

namespace Bddify.Scanners.Modules
{
    public interface IStepScannersModule : IModule
    {
        IEnumerable<IStepScanner> CreateScanners(object testObject);
    }
}