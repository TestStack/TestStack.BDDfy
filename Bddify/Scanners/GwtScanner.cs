using System.Collections.Generic;
using System.Reflection;
using Bddify.Core;

namespace Bddify.Scanners
{
    public class GwtScanner : DefaultScannerBase
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

        protected override IEnumerable<ExecutionStep> ScanForSteps()
        {
            var methods = TestObject.GetType().GetMethods(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

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
                        yield return new ExecutionStep(method, inputs, NetToString.FromName(method.Name), _methodNamingConvention[conventionKey]);

                        if (argAttributes != null && argAttributes.Length > 1)
                        {
                            for (int index = 1; index < argAttributes.Length; index++)
                            {
                                var argAttribute = argAttributes[index];
                                inputs = argAttribute.InputArguments;
                                if (inputs != null && inputs.Length > 0)
                                    yield return new ExecutionStep(method, inputs, NetToString.FromName(method.Name), _methodNamingConvention[conventionKey]);
                            }
                        }

                        break;
                    }
                }
            }

            yield break;
        }
    }
}