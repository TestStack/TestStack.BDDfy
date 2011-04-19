using System;

namespace Bddify.Scanners
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class IgnoreStepAttribute : Attribute
    {
    }
}