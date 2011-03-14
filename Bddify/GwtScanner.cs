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

            return targetMethods.Keys.Select(m => new ExecutionStep(m, NetToString.CreateSentenceFromName(m.Name), _methodNamingConvention[targetMethods[m]]));
        }
    }
}