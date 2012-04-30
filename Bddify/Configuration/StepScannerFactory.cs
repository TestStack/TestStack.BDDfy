using System;
using Bddify.Scanners.StepScanners;

namespace Bddify.Configuration
{
    public class StepScannerFactory : ComponentFactory<IStepScanner, object>
    {
        internal StepScannerFactory(Func<IStepScanner> factory)
            : base(factory)
        {
        }

        internal StepScannerFactory(Func<IStepScanner> factory, bool active)
            : base(factory, active)
        {
        }
    }
}