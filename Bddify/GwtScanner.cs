using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

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
                        var argAttribute = (WithArgsAttribute)method.GetCustomAttributes(typeof(WithArgsAttribute), false).FirstOrDefault();
                        object[] inputs = null;
                        if (argAttribute != null)
                            inputs = argAttribute.InputArguments;

                        yield return new ExecutionStep(method, inputs, NetToString.CreateSentenceFromName(method.Name), _methodNamingConvention[conventionKey]);
                        break;
                    }
                }
            }

            yield break; ;
        }

    }
}