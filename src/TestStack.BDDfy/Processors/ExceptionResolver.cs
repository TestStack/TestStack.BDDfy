using System;
using System.Reflection;

namespace TestStack.BDDfy.Processors
{
    internal class ExceptionResolver
    {
        public static Exception Resolve(Exception ex)
            => ex is not TargetInvocationException ? ex : ex.InnerException ?? ex;
    }
}
