using System.Collections.Generic;
using Bddify.Module;
using Bddify.Scanners.StepScanners;
using Bddify.Scanners.StepScanners.ExecutableAttribute;
using Bddify.Scanners.StepScanners.MethodName;

namespace Bddify.Scanners.Modules
{
    public class DefaultStepScannersModule : DefaultModule, IStepScannersModule
    {
        public IEnumerable<IStepScanner> CreateScanners(object testObject)
        {
            yield return new ExecutableAttributeStepScanner();
            yield return new DefaultMethodNameStepScanner(testObject);
        }
    }
}