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
                        var argAttributes = (WithArgsAttribute[])method.GetCustomAttributes(typeof(WithArgsAttribute), false);
                        object[] inputs = null;
                        if (argAttributes != null && argAttributes.Length > 0)
                            inputs = argAttributes[0].InputArguments;

                        // creating the method itself
                        yield return new ExecutionStep(method, inputs, NetToString.CreateSentenceFromName(method.Name), _methodNamingConvention[conventionKey]);

                        if (argAttributes != null && argAttributes.Length > 1)
                        {
                            for (int index = 1; index < argAttributes.Length; index++)
                            {
                                var argAttribute = argAttributes[index];
                                inputs = argAttribute.InputArguments;
                                if (inputs != null && inputs.Length > 0)
                                    yield return new ExecutionStep(method, inputs, NetToString.CreateSentenceFromName(method.Name), _methodNamingConvention[conventionKey]);
                            }
                        }

                        break;
                    }
                }
            }

            yield break; ;
        }

    }
}