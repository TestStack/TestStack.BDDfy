using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace TestStack.BDDfy
{
    /// <summary>
    /// Uses reflection to scan a scenario class for steps using method name conventions
    /// </summary>
    /// <remarks>
    /// Method names starting with the following words are considered as steps and are
    /// reported: 
    /// <list type="bullet">
    /// <item>
    /// <description><i>Given: </i>setup step </description></item>
    /// <item>
    /// <description><i>AndGiven: </i>setup step running after 'Given' steps
    /// </description></item>
    /// <item>
    /// <description><i>When: </i>state transition step </description></item>
    /// <item>
    /// <description><i>AndWhen: </i>state transition step running after 'When' steps
    /// </description></item>
    /// <item>
    /// <description><i>Then: </i>asserting step </description></item>
    /// <item>
    /// <description><i>And: </i>asserting step running after 'Then' steps
    /// </description></item></list>
    /// <para>A method ending with <i>Context </i>is considered as a setup method (not
    /// reported). </para>
    /// <para>A method starting with <i>Setup </i>is considered as a setup method (not
    /// reported). </para>
    /// <para>A method starting with <i>TearDown </i>is considered as a finally method
    /// which is run after all the other steps (not reported). </para>
    /// </remarks>
    public class MethodNameStepScanner : IStepScanner
    {
        private readonly Func<string, string> _stepTextTransformer;
        private readonly MethodNameMatcher[] _matchers;

        public MethodNameStepScanner(Func<string, string> stepTextTransformer, params MethodNameMatcher[] matchers)
        {
            _stepTextTransformer = stepTextTransformer;
            _matchers = matchers;
        }

        public IEnumerable<ExecutionStep> Scan(object testObject, MethodInfo method)
        {
            foreach (var matcher in _matchers)
            {
                if (!matcher.IsMethodOfInterest(method.Name)) 
                    continue;

                var argAttributes = (RunStepWithArgsAttribute[])method.GetCustomAttributes(typeof(RunStepWithArgsAttribute), false);
                var returnsItsText = method.ReturnType == typeof(IEnumerable<string>);

                if (argAttributes.Length == 0)
                    yield return GetStep(testObject, matcher, method, returnsItsText);

                foreach (var argAttribute in argAttributes)
                {
                    var inputs = argAttribute.InputArguments;
                    if (inputs != null && inputs.Length > 0)
                        yield return GetStep(testObject, matcher, method, returnsItsText, inputs, argAttribute);
                }

                yield break;
            }
        }

        private ExecutionStep GetStep(object testObject, MethodNameMatcher matcher, MethodInfo method, bool returnsItsText, object[] inputs = null, RunStepWithArgsAttribute argAttribute = null)
        {
            var stepMethodName = GetStepTitle(method, testObject, argAttribute, returnsItsText);
            var stepAction = GetStepAction(method, inputs, returnsItsText);
            return new ExecutionStep(stepAction, stepMethodName, matcher.Asserts, matcher.ExecutionOrder, matcher.ShouldReport);
        }

        private string GetStepTitle(MethodInfo method, object testObject, RunStepWithArgsAttribute argAttribute, bool returnsItsText)
        {
            Func<string> stepTitleFromMethodName = () => GetStepTitleFromMethodName(method, argAttribute);

            if(returnsItsText)
                return GetStepTitleFromMethod(method, argAttribute, testObject) ?? stepTitleFromMethodName();

            return stepTitleFromMethodName();
        }

        private string GetStepTitleFromMethodName(MethodInfo method, RunStepWithArgsAttribute argAttribute)
        {
            var methodName = _stepTextTransformer(NetToString.Convert(method.Name));
            object[] inputs = null;

            if (argAttribute != null && argAttribute.InputArguments != null)
                inputs = argAttribute.InputArguments;

            if (inputs == null)
                return methodName;
            
            if (string.IsNullOrEmpty(argAttribute.StepTextTemplate))
            {
                var stringFlatInputs = inputs.FlattenArrays().Select(i => i.ToString()).ToArray();
                return methodName + " " + string.Join(", ", stringFlatInputs);
            }

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

            return StepActionFactory.GetStepAction(method, inputs);
        }

        private static IEnumerable<string> InvokeIEnumerableMethod(MethodInfo method, object testObject, object[] inputs)
        {
            return (IEnumerable<string>)method.Invoke(testObject, inputs);
        }
    }
}
