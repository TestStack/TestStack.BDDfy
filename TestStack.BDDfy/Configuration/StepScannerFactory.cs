using System;
using TestStack.BDDfy.Scanners.StepScanners;

namespace TestStack.BDDfy.Configuration
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