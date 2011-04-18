using System;

namespace Bddify.Scanners
{
    public class DefaultScanForStepsByMethodName : ScanForStepsByMethodName
    {
        public DefaultScanForStepsByMethodName()
            : base(new[]{
                            new MethodNameMatcher(s => s.EndsWith("Context", StringComparison.OrdinalIgnoreCase), false),
                            new MethodNameMatcher(s => s.Equals("Setup", StringComparison.OrdinalIgnoreCase), false),
                            new MethodNameMatcher(s => s.StartsWith("Given", StringComparison.OrdinalIgnoreCase), false),
                            new MethodNameMatcher(s => s.StartsWith("AndGiven", StringComparison.OrdinalIgnoreCase), false),
                            new MethodNameMatcher(s => s.StartsWith("When", StringComparison.OrdinalIgnoreCase), false),
                            new MethodNameMatcher(s => s.StartsWith("AndWhen", StringComparison.OrdinalIgnoreCase), false),
                            new MethodNameMatcher(s => s.StartsWith("Then", StringComparison.OrdinalIgnoreCase), true),
                            new MethodNameMatcher(s => s.StartsWith("And", StringComparison.OrdinalIgnoreCase), true)
                        })
        {
        }
    }
}