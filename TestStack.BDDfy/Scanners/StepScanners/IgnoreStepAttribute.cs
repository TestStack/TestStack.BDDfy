using System;

namespace TestStack.BDDfy.Scanners
{
    /// <summary>
    /// A method attribute used to specify to the ExecutableAttributeStepScanner that it should ignore a method as a step
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class IgnoreStepAttribute : Attribute
    {
    }
}