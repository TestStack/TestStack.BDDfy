using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Bddify
{
    public class GwtScanner : IScanner
    {
        readonly Dictionary<string, bool> _methodNamingConvention =
            new Dictionary<string, bool>
                { 
                    {"Given", false}, 
                    {"AndGiven", false} ,
                    {"When", false}, 
                    {"AndWhen", false}, 
                    {"Then", true}, 
                    {"And", true} 
                };

        public virtual IEnumerable<ExecutionStep> Scan(Type typeToScan)
        {
            var methodsByAttribute = ScanByAttribute(typeToScan);

            // Executable attribute is prefered
            if (methodsByAttribute.Any())
                return methodsByAttribute;

            return ScanByNamingConvention(typeToScan);
        }

        protected virtual IEnumerable<ExecutionStep> ScanByAttribute(Type typeToScan)
        {
            var methods = typeToScan
                .GetMethods(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(m => m.GetCustomAttributes(typeof(ExecutableAttribute), false).Any())
                .OrderBy(m => ((ExecutableAttribute)m.GetCustomAttributes(typeof(ExecutableAttribute), false)[0]).Order);

            return methods.Select(m => new ExecutionStep(m, Bddifier.CreateSentenceFromName(m.Name), IsAssertingByAttribute(m)));
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

        protected virtual IEnumerable<ExecutionStep> ScanByNamingConvention(Type typeToScan)
        {
            var methods = typeToScan.GetMethods(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var targetMethods = new Dictionary<MethodInfo, string>();

            foreach (var conventionKey in _methodNamingConvention.Keys)
            {
                foreach (var method in methods)
                {
                    if (method.Name.StartsWith(conventionKey))
                    {
                        targetMethods.Add(method, conventionKey);
                        break;
                    }
                }
            }

            return targetMethods.Keys.Select(m => new ExecutionStep(m, Bddifier.CreateSentenceFromName(m.Name), _methodNamingConvention[targetMethods[m]]));
        }
    }
}