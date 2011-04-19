using System;
using Bddify.Core;

namespace Bddify.Scanners
{
    public class DefaultScanForStepsByMethodName : ScanForStepsByMethodName
    {
        public DefaultScanForStepsByMethodName()
            : base(new[]{
                            new MethodNameMatcher(s => s.EndsWith("Context", StringComparison.OrdinalIgnoreCase), false, ExecutionOrder.SetupState),
                            new MethodNameMatcher(s => s.Equals("Setup", StringComparison.OrdinalIgnoreCase), false, ExecutionOrder.SetupState),
                            new MethodNameMatcher(s => s.StartsWith("Given", StringComparison.OrdinalIgnoreCase), false, ExecutionOrder.SetupState),
                            new MethodNameMatcher(s => s.StartsWith("AndGiven", StringComparison.OrdinalIgnoreCase), false, ExecutionOrder.ConsequentSetupState),
                            new MethodNameMatcher(s => s.StartsWith("When", StringComparison.OrdinalIgnoreCase), false, ExecutionOrder.Transition),
                            new MethodNameMatcher(s => s.StartsWith("AndWhen", StringComparison.OrdinalIgnoreCase), false, ExecutionOrder.ConsequentTransition),
                            new MethodNameMatcher(s => s.StartsWith("Then", StringComparison.OrdinalIgnoreCase), true, ExecutionOrder.Assertion),
                            new MethodNameMatcher(s => s.StartsWith("And", StringComparison.OrdinalIgnoreCase), true, ExecutionOrder.ConsequentAssertion)
                        })
        {
        }
    }
}