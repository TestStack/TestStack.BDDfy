using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Linq;
using Bddify.Core;

namespace Bddify.Scanners
{
    public class FluentStepScanner<TScenario> : IInitialStep<TScenario>, IAndGiven<TScenario>, IAndWhen<TScenario>, IAndThen<TScenario>, IScanForSteps
    {
        private readonly List<ExecutionStep> _steps = new List<ExecutionStep>();

        int IScanForSteps.Priority
        {
            get { return 0; }
        }

        IEnumerable<ExecutionStep> IScanForSteps.Scan(Type scenarioType)
        {
            if(scenarioType != typeof(TScenario))
                throw new InvalidOperationException("FluentStepScanner is setup to scan " + typeof(TScenario).Name + " but is being asked to scan " + scenarioType.Name);

            return _steps;
        }

        private void AddStep(Expression<Action<TScenario>> stepAction, string stepTextTemplate, bool asserts, ExecutionOrder executionOrder, bool reports=true)
        {
            var methodInfo = GetMethodInfo(stepAction);
            var inputArguments = stepAction.ExtractConstants().ToArray().FlattenArrays();
            var readableMethodName = NetToString.Convert(methodInfo.Name);
            if (!string.IsNullOrEmpty(stepTextTemplate))
                readableMethodName = string.Format(stepTextTemplate, inputArguments);
            else
                readableMethodName = readableMethodName + " " + string.Join(", ", inputArguments);

            readableMethodName = readableMethodName.Trim();

            _steps.Add(new ExecutionStep(o => stepAction.Compile()((TScenario)o), readableMethodName, asserts, executionOrder, reports));
        }

        public IGiven<TScenario> Given(Expression<Action<TScenario>> givenStep, string stepTextTemplate = null)
        {
            AddStep(givenStep, stepTextTemplate, false, ExecutionOrder.SetupState);
            return this;
        }

        IWhen<TScenario> IInitialStep<TScenario>.When(Expression<Action<TScenario>> whenStep, string stepTextTemplate)
        {
            AddWhenStep(whenStep, stepTextTemplate);
            return this;
        }

        private void AddWhenStep(Expression<Action<TScenario>> whenStep, string stepTextTemplate = null)
        {
            AddStep(whenStep, stepTextTemplate, false, ExecutionOrder.Transition);
        }

        IAndGiven<TScenario> IGiven<TScenario>.And(Expression<Action<TScenario>> andGivenStep, string stepTextTemplate)
        {
            AddStep(andGivenStep, stepTextTemplate, false, ExecutionOrder.ConsecutiveSetupState);
            return this;
        }

        IThen<TScenario> IWhen<TScenario>.Then(Expression<Action<TScenario>> thenStep, string stepTextTemplate)
        {
            AddThenStep(thenStep, stepTextTemplate);
            return this;
        }

        IAndWhen<TScenario> IWhen<TScenario>.And(Expression<Action<TScenario>> andWhenStep, string stepTextTemplate)
        {
            AddStep(andWhenStep, stepTextTemplate, false, ExecutionOrder.ConsecutiveTransition);
            return this;
        }

        IThen<TScenario> IGiven<TScenario>.Then(Expression<Action<TScenario>> thenStep, string stepTextTemplate)
        {
            AddThenStep(thenStep, stepTextTemplate);
            return this;
        }

        private void AddThenStep(Expression<Action<TScenario>> thenStep, string stepTextTemplate = null)
        {
            AddStep(thenStep, stepTextTemplate, true, ExecutionOrder.Assertion);
        }

        IWhen<TScenario> IGiven<TScenario>.When(Expression<Action<TScenario>> whenStep, string stepTextTemplate)
        {
            AddWhenStep(whenStep, stepTextTemplate);
            return this;
        }

        IThen<TScenario> IInitialStep<TScenario>.Then(Expression<Action<TScenario>> thenStep, string stepTextTemplate)
        {
            AddThenStep(thenStep, stepTextTemplate);
            return this;
        }

        IAndThen<TScenario> IThen<TScenario>.And(Expression<Action<TScenario>> andThenStep, string stepTextTemplate)
        {
            AddStep(andThenStep, stepTextTemplate, true, ExecutionOrder.ConsecutiveAssertion);
            return this;
        }

        IScanForSteps IFluentScanner<TScenario>.TearDownWith(Expression<Action<TScenario>> tearDownStep)
        {
            AddStep(tearDownStep, null, false, ExecutionOrder.TearDown, false);
            return this;
        }

        private static MethodInfo GetMethodInfo(Expression<Action<TScenario>> stepAction)
        {
            var methodCall = (MethodCallExpression)stepAction.Body;
            return methodCall.Method;
        }

        //private static object[] GetMethodArguments(Expression<Action<TScenario>> stepAction)
        //{
        //    var methodCall = (MethodCallExpression)stepAction.Body;
        //    var arguments = new List<object>();
        //    foreach (var expression in methodCall.Arguments)
        //    {
        //        var constArg = expression as ConstantExpression;
        //        if (constArg == null)
        //        {
        //            var memberExpression = expression as MemberExpression;
        //            if (memberExpression != null)
        //            {
        //                constArg = memberExpression.Expression as ConstantExpression;
        //            }
        //        }

        //        if (constArg != null)
        //        {
        //            arguments.Add(constArg.Value);
        //            continue;
        //        }

        //        var arrayArg = expression as NewArrayExpression;
        //        if (arrayArg != null)
        //        {
        //            arguments.Add(GetMethodArrayArgument(arrayArg));
        //            continue;
        //        }
        //    }
        //    return arguments.ToArray();
        //}

        //private static object GetMethodArrayArgument(NewArrayExpression arrayExpression)
        //{
        //    var arrayElements = new ArrayList();
        //    Type type = arrayExpression.Type.GetElementType();
        //    foreach (var arrayElementExpression in arrayExpression.Expressions)
        //    {
        //        var arrayElement = ((ConstantExpression)arrayElementExpression).Value;
        //        arrayElements.Add(Convert.ChangeType(arrayElement, arrayElementExpression.Type));
        //    }

        //    return arrayElements.ToArray(type);
        //}

        [DebuggerHidden]
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        [DebuggerHidden]
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        [DebuggerHidden]
        public override string ToString()
        {
            return base.ToString();
        }
    }

    internal interface IInitialStep<TScenario>
    {
        IGiven<TScenario> Given(Expression<Action<TScenario>> givenStep, string stepTextTemplate = null);
        IWhen<TScenario> When(Expression<Action<TScenario>> whenStep, string stepTextTemplate = null);
        IThen<TScenario> Then(Expression<Action<TScenario>> thenStep, string stepTextTemplate = null);
    }

    public interface IFluentScanner<TScenario> : IScanForSteps
    {
        IScanForSteps TearDownWith(Expression<Action<TScenario>> tearDownStep);
    }

    public interface IAndGiven<TScenario> : IGiven<TScenario>
    {
    }

    public interface IGiven<TScenario> : IFluentScanner<TScenario>
    {
        IWhen<TScenario> When(Expression<Action<TScenario>> whenStep, string stepTextTemplate = null);
        IAndGiven<TScenario> And(Expression<Action<TScenario>> andGivenStep, string stepTextTemplate = null);
        IThen<TScenario> Then(Expression<Action<TScenario>> thenStep, string stepTextTemplate = null);
    }

    public interface IThen<TScenario> : IFluentScanner<TScenario>
    {
        IAndThen<TScenario> And(Expression<Action<TScenario>> andThenStep, string stepTextTemplate = null);
    }

    public interface IAndThen<TScenario> : IThen<TScenario>
    {
    }

    public interface IWhen<TScenario> : IFluentScanner<TScenario>
    {
        IAndWhen<TScenario> And(Expression<Action<TScenario>> andWhenStep, string stepTextTemplate = null);
        IThen<TScenario> Then(Expression<Action<TScenario>> thenStep, string stepTextTemplate = null);
    }

    public interface IAndWhen<TScenario> : IWhen<TScenario>
    {
    }
}