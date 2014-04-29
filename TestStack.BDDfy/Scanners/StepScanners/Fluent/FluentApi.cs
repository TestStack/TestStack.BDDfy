using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
 
// ReSharper disable CheckNamespace
// This is in BDDfy namespace to make its usage simpler
namespace TestStack.BDDfy
// ReSharper restore CheckNamespace
{
    public static class FluentStepScannerExtensions
    {
        public static IFluentTestBuilder<TScenario> Given<TScenario>(this TScenario testObject, Expression<Action<TScenario>> step, string stepTextTemplate)
            where TScenario: class
        {
            return new FluentTestBuilder<TScenario>(testObject).Given(step, stepTextTemplate);
        }
 
        public static IFluentTestBuilder<TScenario> Given<TScenario>(this TScenario testObject, Expression<Action<TScenario>> step, bool includeInputsInStepTitle)
            where TScenario: class
        {
            return new FluentTestBuilder<TScenario>(testObject).Given(step, includeInputsInStepTitle);
        }
 
        public static IFluentTestBuilder<TScenario> Given<TScenario>(this TScenario testObject, Expression<Action<TScenario>> step)
            where TScenario: class
        {
            return new FluentTestBuilder<TScenario>(testObject).Given(step);
        }
        
        public static IFluentTestBuilder<TScenario> Given<TScenario>(this TScenario testObject, Expression<Func<TScenario, Task>> step, string stepTextTemplate)
            where TScenario: class
        {
            return new FluentTestBuilder<TScenario>(testObject).Given(step, stepTextTemplate);
        }
 
        public static IFluentTestBuilder<TScenario> Given<TScenario>(this TScenario testObject, Expression<Func<TScenario, Task>> step, bool includeInputsInStepTitle)
            where TScenario: class
        {
            return new FluentTestBuilder<TScenario>(testObject).Given(step, includeInputsInStepTitle);
        }
 
        public static IFluentTestBuilder<TScenario> Given<TScenario>(this TScenario testObject, Expression<Func<TScenario, Task>> step)
            where TScenario: class
        {
            return new FluentTestBuilder<TScenario>(testObject).Given(step);
        }
 
        public static IFluentTestBuilder<TScenario> Given<TScenario>(this TScenario testObject, Action step, string title)
            where TScenario : class
        {
            return new FluentTestBuilder<TScenario>(testObject).Given(step, title);
        }
 
        public static IFluentTestBuilder<TScenario> Given<TScenario>(this TScenario testObject, Func<Task> step, string title)
            where TScenario : class
        {
            return new FluentTestBuilder<TScenario>(testObject).Given(step, title);
        }
 
        public static IFluentTestBuilder<TScenario> Given<TScenario>(this TScenario testObject, string title)
            where TScenario : class
        {
            return new FluentTestBuilder<TScenario>(testObject).Given(title);
        }
    }

    public interface IFluentTestBuilder<TScenario> where TScenario: class
    {
        TScenario TestObject { get; }

        IFluentTestBuilder<TScenario> Given(Expression<Action<TScenario>> step, string stepTextTemplate);

        IFluentTestBuilder<TScenario> Given(Expression<Action<TScenario>> step, bool includeInputsInStepTitle);

        IFluentTestBuilder<TScenario> Given(Expression<Action<TScenario>> step);

        IFluentTestBuilder<TScenario> Given(Expression<Func<TScenario, Task>> step, string stepTextTemplate);

        IFluentTestBuilder<TScenario> Given(Expression<Func<TScenario, Task>> step, bool includeInputsInStepTitle);

        IFluentTestBuilder<TScenario> Given(Expression<Func<TScenario, Task>> step);

        IFluentTestBuilder<TScenario> Given(Action step, string title);

        IFluentTestBuilder<TScenario> Given(Func<Task> step, string title);

        IFluentTestBuilder<TScenario> Given(string title);

        IFluentTestBuilder<TScenario> When(Expression<Action<TScenario>> step, string stepTextTemplate);

        IFluentTestBuilder<TScenario> When(Expression<Action<TScenario>> step, bool includeInputsInStepTitle);

        IFluentTestBuilder<TScenario> When(Expression<Action<TScenario>> step);

        IFluentTestBuilder<TScenario> When(Expression<Func<TScenario, Task>> step, string stepTextTemplate);

        IFluentTestBuilder<TScenario> When(Expression<Func<TScenario, Task>> step, bool includeInputsInStepTitle);

        IFluentTestBuilder<TScenario> When(Expression<Func<TScenario, Task>> step);

        IFluentTestBuilder<TScenario> When(Action step, string title);

        IFluentTestBuilder<TScenario> When(Func<Task> step, string title);

        IFluentTestBuilder<TScenario> When(string title);

        IFluentTestBuilder<TScenario> Then(Expression<Action<TScenario>> step, string stepTextTemplate);

        IFluentTestBuilder<TScenario> Then(Expression<Action<TScenario>> step, bool includeInputsInStepTitle);

        IFluentTestBuilder<TScenario> Then(Expression<Action<TScenario>> step);

        IFluentTestBuilder<TScenario> Then(Expression<Func<TScenario, Task>> step, string stepTextTemplate);

        IFluentTestBuilder<TScenario> Then(Expression<Func<TScenario, Task>> step, bool includeInputsInStepTitle);

        IFluentTestBuilder<TScenario> Then(Expression<Func<TScenario, Task>> step);

        IFluentTestBuilder<TScenario> Then(Action step, string title);

        IFluentTestBuilder<TScenario> Then(Func<Task> step, string title);

        IFluentTestBuilder<TScenario> Then(string title);

        IFluentTestBuilder<TScenario> And(Expression<Action<TScenario>> step, string stepTextTemplate);

        IFluentTestBuilder<TScenario> And(Expression<Action<TScenario>> step, bool includeInputsInStepTitle);

        IFluentTestBuilder<TScenario> And(Expression<Action<TScenario>> step);

        IFluentTestBuilder<TScenario> And(Expression<Func<TScenario, Task>> step, string stepTextTemplate);

        IFluentTestBuilder<TScenario> And(Expression<Func<TScenario, Task>> step, bool includeInputsInStepTitle);

        IFluentTestBuilder<TScenario> And(Expression<Func<TScenario, Task>> step);

        IFluentTestBuilder<TScenario> And(Action step, string title);

        IFluentTestBuilder<TScenario> And(Func<Task> step, string title);

        IFluentTestBuilder<TScenario> And(string title);

        IFluentTestBuilder<TScenario> But(Expression<Action<TScenario>> step, string stepTextTemplate);

        IFluentTestBuilder<TScenario> But(Expression<Action<TScenario>> step, bool includeInputsInStepTitle);

        IFluentTestBuilder<TScenario> But(Expression<Action<TScenario>> step);

        IFluentTestBuilder<TScenario> But(Expression<Func<TScenario, Task>> step, string stepTextTemplate);

        IFluentTestBuilder<TScenario> But(Expression<Func<TScenario, Task>> step, bool includeInputsInStepTitle);

        IFluentTestBuilder<TScenario> But(Expression<Func<TScenario, Task>> step);

        IFluentTestBuilder<TScenario> But(Action step, string title);

        IFluentTestBuilder<TScenario> But(Func<Task> step, string title);

        IFluentTestBuilder<TScenario> But(string title);

        IFluentTestBuilder<TScenario> TearDownWith(Expression<Action<TScenario>> step, string stepTextTemplate);

        IFluentTestBuilder<TScenario> TearDownWith(Expression<Action<TScenario>> step, bool includeInputsInStepTitle);

        IFluentTestBuilder<TScenario> TearDownWith(Expression<Action<TScenario>> step);

        IFluentTestBuilder<TScenario> TearDownWith(Expression<Func<TScenario, Task>> step, string stepTextTemplate);

        IFluentTestBuilder<TScenario> TearDownWith(Expression<Func<TScenario, Task>> step, bool includeInputsInStepTitle);

        IFluentTestBuilder<TScenario> TearDownWith(Expression<Func<TScenario, Task>> step);

        IFluentTestBuilder<TScenario> TearDownWith(Action step, string title);

        IFluentTestBuilder<TScenario> TearDownWith(Func<Task> step, string title);

        IFluentTestBuilder<TScenario> TearDownWith(string title);
    }

    interface IFluentTestBuilder
    {
        object TestObject { get; }
    }

    public class FluentTestBuilder<TScenario> : IFluentTestBuilder<TScenario>, IFluentTestBuilder 
                                                where TScenario : class
    {
        readonly FluentScanner<TScenario> scanner;

        public FluentTestBuilder(TScenario testObject)
        {
            TestObject = testObject;
            var existingContext = TestContext.GetContext(TestObject);
            if (existingContext.FluentScanner == null)
                existingContext.FluentScanner = new FluentScanner<TScenario>(testObject);
 
            scanner = (FluentScanner<TScenario>) existingContext.FluentScanner;
        }

        public TScenario TestObject { get; private set; }

        object IFluentTestBuilder.TestObject { get { return TestObject; } }

        public IFluentTestBuilder<TScenario> Given(Expression<Action<TScenario>> step, string stepTextTemplate)
        {
            scanner.AddStep(step, stepTextTemplate, true, true, ExecutionOrder.SetupState, false);
            return this;
        }

        public IFluentTestBuilder<TScenario> Given(Expression<Action<TScenario>> step, bool includeInputsInStepTitle)
        {
            scanner.AddStep(step, null, includeInputsInStepTitle, true, ExecutionOrder.SetupState, false);
            return this;
        }

        public IFluentTestBuilder<TScenario> Given(Expression<Action<TScenario>> step)
        {
            scanner.AddStep(step, null, true, true, ExecutionOrder.SetupState, false);
            return this;
        }

        public IFluentTestBuilder<TScenario> Given(Expression<Func<TScenario, Task>> step, string stepTextTemplate)
        {
            scanner.AddStep(step, stepTextTemplate, true, true, ExecutionOrder.SetupState, false);
            return this;
        }

        public IFluentTestBuilder<TScenario> Given(Expression<Func<TScenario, Task>> step, bool includeInputsInStepTitle)
        {
            scanner.AddStep(step, null, includeInputsInStepTitle, true, ExecutionOrder.SetupState, false);
            return this;
        }

        public IFluentTestBuilder<TScenario> Given(Expression<Func<TScenario, Task>> step)
        {
            scanner.AddStep(step, null, true, true, ExecutionOrder.SetupState, false);
            return this;
        }

        public IFluentTestBuilder<TScenario> Given(Action step, string title)
        {
            scanner.AddStep(step, title, true, ExecutionOrder.SetupState, false);
            return this;
        }

        public IFluentTestBuilder<TScenario> Given(Func<Task> step, string title)
        {
            scanner.AddStep(step, title, true, ExecutionOrder.SetupState, false);
            return this;
        }

        public IFluentTestBuilder<TScenario> Given(string title)
        {
            scanner.AddStep(() => { }, title, true, ExecutionOrder.SetupState, false);
            return this;
        }
        public IFluentTestBuilder<TScenario> When(Expression<Action<TScenario>> step, string stepTextTemplate)
        {
            scanner.AddStep(step, stepTextTemplate, true, true, ExecutionOrder.Transition, false);
            return this;
        }

        public IFluentTestBuilder<TScenario> When(Expression<Action<TScenario>> step, bool includeInputsInStepTitle)
        {
            scanner.AddStep(step, null, includeInputsInStepTitle, true, ExecutionOrder.Transition, false);
            return this;
        }

        public IFluentTestBuilder<TScenario> When(Expression<Action<TScenario>> step)
        {
            scanner.AddStep(step, null, true, true, ExecutionOrder.Transition, false);
            return this;
        }

        public IFluentTestBuilder<TScenario> When(Expression<Func<TScenario, Task>> step, string stepTextTemplate)
        {
            scanner.AddStep(step, stepTextTemplate, true, true, ExecutionOrder.Transition, false);
            return this;
        }

        public IFluentTestBuilder<TScenario> When(Expression<Func<TScenario, Task>> step, bool includeInputsInStepTitle)
        {
            scanner.AddStep(step, null, includeInputsInStepTitle, true, ExecutionOrder.Transition, false);
            return this;
        }

        public IFluentTestBuilder<TScenario> When(Expression<Func<TScenario, Task>> step)
        {
            scanner.AddStep(step, null, true, true, ExecutionOrder.Transition, false);
            return this;
        }

        public IFluentTestBuilder<TScenario> When(Action step, string title)
        {
            scanner.AddStep(step, title, true, ExecutionOrder.Transition, false);
            return this;
        }

        public IFluentTestBuilder<TScenario> When(Func<Task> step, string title)
        {
            scanner.AddStep(step, title, true, ExecutionOrder.Transition, false);
            return this;
        }

        public IFluentTestBuilder<TScenario> When(string title)
        {
            scanner.AddStep(() => { }, title, true, ExecutionOrder.Transition, false);
            return this;
        }
        public IFluentTestBuilder<TScenario> Then(Expression<Action<TScenario>> step, string stepTextTemplate)
        {
            scanner.AddStep(step, stepTextTemplate, true, true, ExecutionOrder.Assertion, true);
            return this;
        }

        public IFluentTestBuilder<TScenario> Then(Expression<Action<TScenario>> step, bool includeInputsInStepTitle)
        {
            scanner.AddStep(step, null, includeInputsInStepTitle, true, ExecutionOrder.Assertion, true);
            return this;
        }

        public IFluentTestBuilder<TScenario> Then(Expression<Action<TScenario>> step)
        {
            scanner.AddStep(step, null, true, true, ExecutionOrder.Assertion, true);
            return this;
        }

        public IFluentTestBuilder<TScenario> Then(Expression<Func<TScenario, Task>> step, string stepTextTemplate)
        {
            scanner.AddStep(step, stepTextTemplate, true, true, ExecutionOrder.Assertion, true);
            return this;
        }

        public IFluentTestBuilder<TScenario> Then(Expression<Func<TScenario, Task>> step, bool includeInputsInStepTitle)
        {
            scanner.AddStep(step, null, includeInputsInStepTitle, true, ExecutionOrder.Assertion, true);
            return this;
        }

        public IFluentTestBuilder<TScenario> Then(Expression<Func<TScenario, Task>> step)
        {
            scanner.AddStep(step, null, true, true, ExecutionOrder.Assertion, true);
            return this;
        }

        public IFluentTestBuilder<TScenario> Then(Action step, string title)
        {
            scanner.AddStep(step, title, true, ExecutionOrder.Assertion, true);
            return this;
        }

        public IFluentTestBuilder<TScenario> Then(Func<Task> step, string title)
        {
            scanner.AddStep(step, title, true, ExecutionOrder.Assertion, true);
            return this;
        }

        public IFluentTestBuilder<TScenario> Then(string title)
        {
            scanner.AddStep(() => { }, title, true, ExecutionOrder.Assertion, true);
            return this;
        }
        public IFluentTestBuilder<TScenario> And(Expression<Action<TScenario>> step, string stepTextTemplate)
        {
            scanner.AddStep(step, stepTextTemplate, true, true, ExecutionOrder.ConsecutiveStep, false);
            return this;
        }

        public IFluentTestBuilder<TScenario> And(Expression<Action<TScenario>> step, bool includeInputsInStepTitle)
        {
            scanner.AddStep(step, null, includeInputsInStepTitle, true, ExecutionOrder.ConsecutiveStep, false);
            return this;
        }

        public IFluentTestBuilder<TScenario> And(Expression<Action<TScenario>> step)
        {
            scanner.AddStep(step, null, true, true, ExecutionOrder.ConsecutiveStep, false);
            return this;
        }

        public IFluentTestBuilder<TScenario> And(Expression<Func<TScenario, Task>> step, string stepTextTemplate)
        {
            scanner.AddStep(step, stepTextTemplate, true, true, ExecutionOrder.ConsecutiveStep, false);
            return this;
        }

        public IFluentTestBuilder<TScenario> And(Expression<Func<TScenario, Task>> step, bool includeInputsInStepTitle)
        {
            scanner.AddStep(step, null, includeInputsInStepTitle, true, ExecutionOrder.ConsecutiveStep, false);
            return this;
        }

        public IFluentTestBuilder<TScenario> And(Expression<Func<TScenario, Task>> step)
        {
            scanner.AddStep(step, null, true, true, ExecutionOrder.ConsecutiveStep, false);
            return this;
        }

        public IFluentTestBuilder<TScenario> And(Action step, string title)
        {
            scanner.AddStep(step, title, true, ExecutionOrder.ConsecutiveStep, false);
            return this;
        }

        public IFluentTestBuilder<TScenario> And(Func<Task> step, string title)
        {
            scanner.AddStep(step, title, true, ExecutionOrder.ConsecutiveStep, false);
            return this;
        }

        public IFluentTestBuilder<TScenario> And(string title)
        {
            scanner.AddStep(() => { }, title, true, ExecutionOrder.ConsecutiveStep, false);
            return this;
        }
        public IFluentTestBuilder<TScenario> But(Expression<Action<TScenario>> step, string stepTextTemplate)
        {
            scanner.AddStep(step, stepTextTemplate, true, true, ExecutionOrder.ConsecutiveStep, false);
            return this;
        }

        public IFluentTestBuilder<TScenario> But(Expression<Action<TScenario>> step, bool includeInputsInStepTitle)
        {
            scanner.AddStep(step, null, includeInputsInStepTitle, true, ExecutionOrder.ConsecutiveStep, false);
            return this;
        }

        public IFluentTestBuilder<TScenario> But(Expression<Action<TScenario>> step)
        {
            scanner.AddStep(step, null, true, true, ExecutionOrder.ConsecutiveStep, false);
            return this;
        }

        public IFluentTestBuilder<TScenario> But(Expression<Func<TScenario, Task>> step, string stepTextTemplate)
        {
            scanner.AddStep(step, stepTextTemplate, true, true, ExecutionOrder.ConsecutiveStep, false);
            return this;
        }

        public IFluentTestBuilder<TScenario> But(Expression<Func<TScenario, Task>> step, bool includeInputsInStepTitle)
        {
            scanner.AddStep(step, null, includeInputsInStepTitle, true, ExecutionOrder.ConsecutiveStep, false);
            return this;
        }

        public IFluentTestBuilder<TScenario> But(Expression<Func<TScenario, Task>> step)
        {
            scanner.AddStep(step, null, true, true, ExecutionOrder.ConsecutiveStep, false);
            return this;
        }

        public IFluentTestBuilder<TScenario> But(Action step, string title)
        {
            scanner.AddStep(step, title, true, ExecutionOrder.ConsecutiveStep, false);
            return this;
        }

        public IFluentTestBuilder<TScenario> But(Func<Task> step, string title)
        {
            scanner.AddStep(step, title, true, ExecutionOrder.ConsecutiveStep, false);
            return this;
        }

        public IFluentTestBuilder<TScenario> But(string title)
        {
            scanner.AddStep(() => { }, title, true, ExecutionOrder.ConsecutiveStep, false);
            return this;
        }
        public IFluentTestBuilder<TScenario> TearDownWith(Expression<Action<TScenario>> step, string stepTextTemplate)
        {
            scanner.AddStep(step, stepTextTemplate, true, false, ExecutionOrder.TearDown, false);
            return this;
        }

        public IFluentTestBuilder<TScenario> TearDownWith(Expression<Action<TScenario>> step, bool includeInputsInStepTitle)
        {
            scanner.AddStep(step, null, includeInputsInStepTitle, false, ExecutionOrder.TearDown, false);
            return this;
        }

        public IFluentTestBuilder<TScenario> TearDownWith(Expression<Action<TScenario>> step)
        {
            scanner.AddStep(step, null, true, false, ExecutionOrder.TearDown, false);
            return this;
        }

        public IFluentTestBuilder<TScenario> TearDownWith(Expression<Func<TScenario, Task>> step, string stepTextTemplate)
        {
            scanner.AddStep(step, stepTextTemplate, true, false, ExecutionOrder.TearDown, false);
            return this;
        }

        public IFluentTestBuilder<TScenario> TearDownWith(Expression<Func<TScenario, Task>> step, bool includeInputsInStepTitle)
        {
            scanner.AddStep(step, null, includeInputsInStepTitle, false, ExecutionOrder.TearDown, false);
            return this;
        }

        public IFluentTestBuilder<TScenario> TearDownWith(Expression<Func<TScenario, Task>> step)
        {
            scanner.AddStep(step, null, true, false, ExecutionOrder.TearDown, false);
            return this;
        }

        public IFluentTestBuilder<TScenario> TearDownWith(Action step, string title)
        {
            scanner.AddStep(step, title, false, ExecutionOrder.TearDown, false);
            return this;
        }

        public IFluentTestBuilder<TScenario> TearDownWith(Func<Task> step, string title)
        {
            scanner.AddStep(step, title, false, ExecutionOrder.TearDown, false);
            return this;
        }

        public IFluentTestBuilder<TScenario> TearDownWith(string title)
        {
            scanner.AddStep(() => { }, title, false, ExecutionOrder.TearDown, false);
            return this;
        }
    }
}