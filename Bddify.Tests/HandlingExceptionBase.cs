using System;
using System.Reflection;

namespace Bddify.Tests
{
    public class Helpers
    {
        public static MethodInfo GetMethodInfo(Action methodOn)
        {
            return methodOn.Method;
        }
    }
}