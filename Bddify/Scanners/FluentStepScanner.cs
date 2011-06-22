#if !SILVERLIGHT
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Linq;
using Bddify.Core;

namespace Bddify.Scanners
{
    public class FluentStepScanner<TScenario> : IInitialStep<TScenario>, IAndGiven<TScenario>, IAndWhen<TScenario>, IAndThen<TScenario>
        where TScenario:class, new()
    {
        private readonly List<ExecutionStep> _steps = new List<ExecutionStep>();

        public static IInitialStep<TScenario> Scan()
        {
            return new FluentStepScanner<TScenario>();
        }

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

#if NET35
        public void AddStep(Expression<Action<TScenario>> stepAction, string stepTextTemplate, bool asserts, ExecutionOrder executionOrder)
        {
            AddStep(stepAction, stepTextTemplate, asserts, executionOrder, true);
        }

        public IWhen<TScenario> When(Expression<Action<TScenario>> whenStep)
        {
            AddWhenStep(whenStep, null);
            return this;
        }

        public IGiven<TScenario> Given(Expression<Action<TScenario>> givenStep)
        {
            return Given(givenStep, null);
        }

        IWhen<TScenario> IGiven<TScenario>.When(Expression<Action<TScenario>> whenStep)
        {
            return When(whenStep);
        }

        IThen<TScenario> IWhen<TScenario>.Then(Expression<Action<TScenario>> thenStep)
        {
            AddThenStep(thenStep, null);
            return this;
        }

        IThen<TScenario> IGiven<TScenario>.Then(Expression<Action<TScenario>> thenStep)
        {
            AddThenStep(thenStep, null);
            return this;
        }

        IAndThen<TScenario> IThen<TScenario>.And(Expression<Action<TScenario>> andThenStep)
        {
            return ((IThen<TScenario>)this).And(andThenStep, null);
        }

        IAndWhen<TScenario> IWhen<TScenario>.And(Expression<Action<TScenario>> andWhenStep)
        {
            return ((IWhen<TScenario>)this).And(andWhenStep, null);
        }

        IAndGiven<TScenario> IGiven<TScenario>.And(Expression<Action<TScenario>> andGivenStep)
        {
            return ((IGiven<TScenario>)this).And(andGivenStep, null);
        }

        public Story Bddify(string title)
        {
            return Bddify(title, null, true, true, true);
        }

        public Story Bddify()
        {
            return Bddify(null);
        }
#endif

        public void AddStep(Expression<Action<TScenario>> stepAction, string stepTextTemplate, bool asserts, ExecutionOrder executionOrder, bool reports=true)
        {
            var methodInfo = GetMethodInfo(stepAction);
            var inputArguments = stepAction.ExtractConstants().ToArray();
            var flatInputArray = inputArguments.FlattenArrays();
            var readableMethodName = NetToString.Convert(methodInfo.Name);
            if (!string.IsNullOrEmpty(stepTextTemplate))
                readableMethodName = string.Format(stepTextTemplate, flatInputArray);
            else
                readableMethodName = readableMethodName + " " + string.Join(", ", flatInputArray);

            readableMethodName = readableMethodName.Trim();
            var action = stepAction.Compile();
            _steps.Add(new ExecutionStep(o => action((TScenario)o), readableMethodName, asserts, executionOrder, reports));
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

        IAndThen<TScenario> IThen<TScenario>.And(Expression<Action<TScenario>> andThenStep, string stepTextTemplate)
        {
            AddStep(andThenStep, stepTextTemplate, true, ExecutionOrder.ConsecutiveAssertion);
            return this;
        }

        IFluentScanner<TScenario> IFluentScanner<TScenario>.TearDownWith(Expression<Action<TScenario>> tearDownStep)
        {
            AddStep(tearDownStep, null, false, ExecutionOrder.TearDown, false);
            return this;
        }

        private static MethodInfo GetMethodInfo(Expression<Action<TScenario>> stepAction)
        {
            var methodCall = (MethodCallExpression)stepAction.Body;
            return methodCall.Method;
        }

        public Story Bddify(string title = null, IExceptionProcessor exceptionProcessor = null, bool handleExceptions = true, bool htmlReport = true, bool consoleReport = true)
        {
            if (title == null)
                title = GetTitleFromMethodNameInStackTrace();

            return typeof(TScenario).Bddify(exceptionProcessor, handleExceptions, htmlReport, consoleReport, title, this);
        }

        private static string GetTitleFromMethodNameInStackTrace()
        {
            var trace = new StackTrace();
            var frames = trace.GetFrames();
            if(frames == null)
                return null;

            var initiatingFrame = frames.LastOrDefault(s => s.GetMethod().DeclaringType == typeof(TScenario));
            if (initiatingFrame == null)
                return null;

            return NetToString.Convert(initiatingFrame.GetMethod().Name);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToString()
        {
            return base.ToString();
        }
    }

    public interface IInitialStep<TScenario>
    {
        IGiven<TScenario> Given(Expression<Action<TScenario>> givenStep, string stepTextTemplate = null);
        IWhen<TScenario> When(Expression<Action<TScenario>> whenStep, string stepTextTemplate = null);
#if NET35
        IGiven<TScenario> Given(Expression<Action<TScenario>> givenStep);
        IWhen<TScenario> When(Expression<Action<TScenario>> whenStep);
#endif
    }

    public interface IFluentScanner<TScenario> : IScanForSteps
    {
        IFluentScanner<TScenario> TearDownWith(Expression<Action<TScenario>> tearDownStep);
        Story Bddify(string title = null, IExceptionProcessor exceptionProcessor = null, bool handleExceptions = true, bool htmlReport = true, bool consoleReport = true);
#if NET35
        Story Bddify(string title);
        Story Bddify();
#endif
    }

    public interface IAndGiven<TScenario> : IGiven<TScenario>
    {
    }

    public interface IGiven<TScenario> : IFluentScanner<TScenario>
    {
        IWhen<TScenario> When(Expression<Action<TScenario>> whenStep, string stepTextTemplate = null);
        IAndGiven<TScenario> And(Expression<Action<TScenario>> andGivenStep, string stepTextTemplate = null);
        IThen<TScenario> Then(Expression<Action<TScenario>> thenStep, string stepTextTemplate = null);
#if NET35
        IWhen<TScenario> When(Expression<Action<TScenario>> whenStep);
        IAndGiven<TScenario> And(Expression<Action<TScenario>> andGivenStep);
        IThen<TScenario> Then(Expression<Action<TScenario>> thenStep);
#endif
    }

    public interface IThen<TScenario> : IFluentScanner<TScenario>
    {
        IAndThen<TScenario> And(Expression<Action<TScenario>> andThenStep, string stepTextTemplate = null);
#if NET35
        IAndThen<TScenario> And(Expression<Action<TScenario>> andThenStep);
#endif
    }

    public interface IAndThen<TScenario> : IThen<TScenario>
    {
    }

    public interface IWhen<TScenario> : IFluentScanner<TScenario>
    {
        IAndWhen<TScenario> And(Expression<Action<TScenario>> andWhenStep, string stepTextTemplate = null);
        IThen<TScenario> Then(Expression<Action<TScenario>> thenStep, string stepTextTemplate = null);
#if NET35
        IAndWhen<TScenario> And(Expression<Action<TScenario>> andWhenStep);
        IThen<TScenario> Then(Expression<Action<TScenario>> thenStep);
#endif
    }

    public interface IAndWhen<TScenario> : IWhen<TScenario>
    {
    }
}
#endif
