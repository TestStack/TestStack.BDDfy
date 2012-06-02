using System;
using System.Reflection;

namespace TestStack.BDDfy.Tests
{
    public class Helpers
    {
        public static MethodInfo GetMethodInfo(Action methodOn)
        {
            return methodOn.Method;
        }
    }
}