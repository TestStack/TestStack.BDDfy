using System;
using System.Reflection;

namespace BDDfy.Tests
{
    public class Helpers
    {
        public static MethodInfo GetMethodInfo(Action methodOn)
        {
            return methodOn.Method;
        }
    }
}