using System;
using System.Reflection;

namespace Bddify.Tests.BddifySpecs.Exceptions
{
    public class HandlingExceptionBase
    {
        protected static MethodInfo GetMethodInfo(Action methodOn)
        {
            return methodOn.Method;
        }

    }
}