using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Bddify
{
    public class Scanner : IScanner
    {
        readonly List<string> _methodNamingConvention = new List<string> { "Given", "AndGiven", "When", "AndWhen", "Then", "And" };

        public virtual IEnumerable<MethodInfo> Scan(Type type)
        {
            var methods = type
                .GetMethods(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(m => m.GetCustomAttributes(typeof(ExecutableAttribute), false).Any())
                .OrderBy(m => ((ExecutableAttribute)m.GetCustomAttributes(typeof(ExecutableAttribute), false)[0]).Order);

            // Executable attribute is prefered
            if (methods.Any())
                return methods;

            return ScanByNamingConvention(type);
        }

        private IEnumerable<MethodInfo> ScanByNamingConvention(Type type)
        {
            var methods = type.GetMethods(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var targetMethods = new List<MethodInfo>();

            foreach (var prefix in _methodNamingConvention)
            {
                string prefix1 = prefix;
                targetMethods.AddRange(methods.Where(m => m.Name.StartsWith(prefix1)));
            }

            return targetMethods;
        }
    }
}