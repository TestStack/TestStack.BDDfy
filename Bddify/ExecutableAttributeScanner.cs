using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Bddify
{
    public class ExecutableAttributeScanner : IScanner
    {
        public virtual IEnumerable<ExecutionStep> Scan(Type typeToScan)
        {
            var methods = typeToScan
                .GetMethods(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(m => m.GetCustomAttributes(typeof(ExecutableAttribute), false).Any())
                .OrderBy(m => ((ExecutableAttribute)m.GetCustomAttributes(typeof(ExecutableAttribute), false)[0]).Order);

            // ToDo: the arguments should be provided
            return methods.Select(m => new ExecutionStep(m, null, NetToString.CreateSentenceFromName(m.Name), IsAssertingByAttribute(m)));
        }

        private static bool IsAssertingByAttribute(MethodInfo method)
        {
            var attribute = GetExecutableAttribute(method);
            return attribute.Asserts;
        }

        private static ExecutableAttribute GetExecutableAttribute(MethodInfo method)
        {
            return (ExecutableAttribute)method.GetCustomAttributes(typeof(ExecutableAttribute), false).First();
        }
    }
}