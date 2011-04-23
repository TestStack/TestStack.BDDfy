using System;
using System.Reflection;

namespace Bddify.Tests.MsTest
{
    public class Helpers
    {
        public static MethodInfo GetMethodInfo(Action methodOn)
        {
            return methodOn.Method;
        }
    }
}