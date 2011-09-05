using System;
using System.Collections.Generic;
using System.Reflection;
using Bddify.Core;
using System.Linq;

namespace Bddify.Scanners.StepScanners.MethodName
{
    public class MethodNameStepScanner : IScanForSteps
    {
        private readonly MethodNameMatcher[] _matchers;
        private readonly object _testObject;

        public MethodNameStepScanner(object testObject, MethodNameMatcher[] matchers)
        {
            _testObject = testObject;
            _matchers = matchers;
        }

        public IEnumerable<ExecutionStep> Scan(MethodInfo method)
        {
            foreach (var matcher in _matchers)
            {
                if (!matcher.IsMethodOfInterest(method.Name)) 
                    continue;

                var argAttributes = (RunStepWithArgsAttribute[])method.GetCustomAttributes(typeof(RunStepWithArgsAttribute), false);
                var returnsItsText = method.ReturnType == typeof(IEnumerable<string>);

                if (argAttributes.Length == 0)
                    yield return GetStep(_testObject, matcher, method, returnsItsText);

                foreach (var argAttribute in argAttributes)
                {
                    var inputs = argAttribute.InputArguments;
                    if (inputs != null && inputs.Length > 0)
                        yield return GetStep(_testObject, matcher, method, returnsItsText, inputs, argAttribute);
                }

                yield break;
            }
        }

        private static ExecutionStep GetStep(object testObject, MethodNameMatcher matcher, MethodInfo method, bool returnsItsText, object[] inputs = null, RunStepWithArgsAttribute argAttribute = null)
        {
            var stepMethodName = GetStepTitle(method, testObject, argAttribute, returnsItsText);
            var stepAction = GetStepAction(method, inputs, returnsItsText);
            return new ExecutionStep(stepAction, stepMethodName, matcher.Asserts, matcher.ExecutionOrder, matcher.ShouldReport);
        }

        private static string GetStepTitle(MethodInfo method, object testObject, RunStepWithArgsAttribute argAttribute, bool returnsItsText)
        {
            Func<string> stepTitleFromMethodName = () => GetStepTitleFromMethodName(method, argAttribute);

            if(returnsItsText)
                return GetStepTitleFromMethod(method, argAttribute, testObject) ?? stepTitleFromMethodName();

            return stepTitleFromMethodName();
        }

        private static string GetStepTitleFromMethodName(MethodInfo method, RunStepWithArgsAttribute argAttribute)
        {
            var methodName = CleanupTheStepText(NetToString.Convert(method.Name));
            object[] inputs = null;

            if (argAttribute != null && argAttribute.InputArguments != null)
                inputs = argAttribute.InputArguments;

            if (inputs == null)
                return methodName;
            
            if (string.IsNullOrEmpty(argAttribute.StepTextTemplate))
                return methodName + " " + string.Join(", ", inputs.FlattenArrays());

            return string.Format(argAttribute.StepTextTemplate, inputs.FlattenArrays());
        }

        private static string GetStepTitleFromMethod(MethodInfo method, RunStepWithArgsAttribute argAttribute, object testObject)
        {
            object[] inputs = null;
            if(argAttribute != null && argAttribute.InputArguments != null)
                inputs = argAttribute.InputArguments;

            var enumerableResult = InvokeIEnumerableMethod(method, testObject, inputs);
            try
            {
                return enumerableResult.FirstOrDefault();
            }
            catch (Exception ex)
            {
                var message = string.Format(
                    "The signature of method '{0}' indicates that it returns its step title; but the code is throwing an exception before a title is returned",
                    method.Name);
                throw new StepTitleException(message, ex);
            }
        }

        static Action<object> GetStepAction(MethodInfo method, object[] inputs, bool returnsItsText)
        {
            if (returnsItsText)
            {
                // Note: Count() is a silly trick to enumerate over the method and make sure it returns because it is an IEnumerable method and runs lazily otherwise
                return o => InvokeIEnumerableMethod(method, o, inputs).Count();
            }

            return o => method.Invoke(o, inputs);
        }

        private static IEnumerable<string> InvokeIEnumerableMethod(MethodInfo method, object testObject, object[] inputs)
        {
            return (IEnumerable<string>)method.Invoke(testObject, inputs);
        }

        static string CleanupTheStepText(string stepText)
        {
            if (stepText.StartsWith("and given ", StringComparison.OrdinalIgnoreCase))
                return stepText.Remove("and ".Length, "given ".Length);

            if(stepText.StartsWith("and when ", StringComparison.OrdinalIgnoreCase))
                return stepText.Remove("and ".Length, "when ".Length);

            return stepText;
        }
    }
}