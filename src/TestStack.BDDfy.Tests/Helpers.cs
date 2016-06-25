using System;
using System.Linq.Expressions;
using System.Reflection;

namespace TestStack.BDDfy.Tests
{
    public class Helpers
    {
        public static MethodInfo GetMethodInfo(Expression<Action> methodOn)
        {
            var methodCallExp = (MethodCallExpression)methodOn.Body;
            return methodCallExp.Method;
        }
    }
}