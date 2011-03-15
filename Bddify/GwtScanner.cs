using System;
using System.Collections.Generic;
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

            foreach (var conventionKey in _methodNamingConvention.Keys)
            {
                foreach (var method in methods)
                {
                    if (method.Name.StartsWith(conventionKey))
                    {
                        yield return new ExecutionStep(method, NetToString.CreateSentenceFromName(method.Name), _methodNamingConvention[conventionKey]);
                        break;
                    }
                }
            }

            yield break; ;
        }

    }
}