using System;

namespace Bddify.Scanners.StepScanners
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class IgnoreStepAttribute : Attribute
    {
    }
}