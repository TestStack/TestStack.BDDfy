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
        public static IFluentStepBuilder<TScenario> Given<TScenario>(this TScenario testObject, Expression<Action<TScenario>> step, string stepTextTemplate)
            where TScenario: class
        {
            return new FluentStepBuilder<TScenario>(testObject).Given(step, stepTextTemplate);
        }
 
        public static IFluentStepBuilder<TScenario> Given<TScenario>(this TScenario testObject, Expression<Action<TScenario>> step, bool includeInputsInStepTitle)
            where TScenario: class
        {
            return new FluentStepBuilder<TScenario>(testObject).Given(step, includeInputsInStepTitle);
        }
 
        public static IFluentStepBuilder<TScenario> Given<TScenario>(this TScenario testObject, Expression<Action<TScenario>> step)
            where TScenario: class
        {
            return new FluentStepBuilder<TScenario>(testObject).Given(step);
        }
        
        public static IFluentStepBuilder<TScenario> Given<TScenario>(this TScenario testObject, Expression<Func<TScenario, Task>> step, string stepTextTemplate)
            where TScenario: class
        {
            return new FluentStepBuilder<TScenario>(testObject).Given(step, stepTextTemplate);
        }
 
        public static IFluentStepBuilder<TScenario> Given<TScenario>(this TScenario testObject, Expression<Func<TScenario, Task>> step, bool includeInputsInStepTitle)
            where TScenario: class
        {
            return new FluentStepBuilder<TScenario>(testObject).Given(step, includeInputsInStepTitle);
        }
 
        public static IFluentStepBuilder<TScenario> Given<TScenario>(this TScenario testObject, Expression<Func<TScenario, Task>> step)
            where TScenario: class
        {
            return new FluentStepBuilder<TScenario>(testObject).Given(step);
        }
 
        public static IFluentStepBuilder<TScenario> Given<TScenario>(this TScenario testObject, Action step, string title)
            where TScenario : class
        {
            return new FluentStepBuilder<TScenario>(testObject).Given(step, title);
        }
 
        public static IFluentStepBuilder<TScenario> Given<TScenario>(this TScenario testObject, Func<Task> step, string title)
            where TScenario : class
        {
            return new FluentStepBuilder<TScenario>(testObject).Given(step, title);
        }
 
        public static IFluentStepBuilder<TScenario> Given<TScenario>(this TScenario testObject, string title)
            where TScenario : class
        {
            return new FluentStepBuilder<TScenario>(testObject).Given(title);
        }

        public static IFluentStepBuilder<TScenario> When<TScenario>(this TScenario testObject, Expression<Action<TScenario>> step, string stepTextTemplate)
            where TScenario: class
        {
            return new FluentStepBuilder<TScenario>(testObject).When(step, stepTextTemplate);
        }
 
        public static IFluentStepBuilder<TScenario> When<TScenario>(this TScenario testObject, Expression<Action<TScenario>> step, bool includeInputsInStepTitle)
            where TScenario: class
        {
            return new FluentStepBuilder<TScenario>(testObject).When(step, includeInputsInStepTitle);
        }
 
        public static IFluentStepBuilder<TScenario> When<TScenario>(this TScenario testObject, Expression<Action<TScenario>> step)
            where TScenario: class
        {
            return new FluentStepBuilder<TScenario>(testObject).When(step);
        }
        
        public static IFluentStepBuilder<TScenario> When<TScenario>(this TScenario testObject, Expression<Func<TScenario, Task>> step, string stepTextTemplate)
            where TScenario: class
        {
            return new FluentStepBuilder<TScenario>(testObject).When(step, stepTextTemplate);
        }
 
        public static IFluentStepBuilder<TScenario> When<TScenario>(this TScenario testObject, Expression<Func<TScenario, Task>> step, bool includeInputsInStepTitle)
            where TScenario: class
        {
            return new FluentStepBuilder<TScenario>(testObject).When(step, includeInputsInStepTitle);
        }
 
        public static IFluentStepBuilder<TScenario> When<TScenario>(this TScenario testObject, Expression<Func<TScenario, Task>> step)
            where TScenario: class
        {
            return new FluentStepBuilder<TScenario>(testObject).When(step);
        }
 
        public static IFluentStepBuilder<TScenario> When<TScenario>(this TScenario testObject, Action step, string title)
            where TScenario : class
        {
            return new FluentStepBuilder<TScenario>(testObject).When(step, title);
        }
 
        public static IFluentStepBuilder<TScenario> When<TScenario>(this TScenario testObject, Func<Task> step, string title)
            where TScenario : class
        {
            return new FluentStepBuilder<TScenario>(testObject).When(step, title);
        }
 
        public static IFluentStepBuilder<TScenario> When<TScenario>(this TScenario testObject, string title)
            where TScenario : class
        {
            return new FluentStepBuilder<TScenario>(testObject).When(title);
        }
    }

    public interface IFluentStepBuilder<TScenario> where TScenario: class
    {
        TScenario TestObject { get; }

        IFluentStepBuilder<TScenario> Given(Expression<Action<TScenario>> step, string stepTextTemplate);

        IFluentStepBuilder<TScenario> Given(Expression<Action<TScenario>> step, bool includeInputsInStepTitle);

        IFluentStepBuilder<TScenario> Given(Expression<Action<TScenario>> step);

        IFluentStepBuilder<TScenario> Given(Expression<Func<TScenario, Task>> step, string stepTextTemplate);

        IFluentStepBuilder<TScenario> Given(Expression<Func<TScenario, Task>> step, bool includeInputsInStepTitle);

        IFluentStepBuilder<TScenario> Given(Expression<Func<TScenario, Task>> step);

        IFluentStepBuilder<TScenario> Given(Action step, string title);

        IFluentStepBuilder<TScenario> Given(Func<Task> step, string title);

        IFluentStepBuilder<TScenario> Given(string title);

        IFluentStepBuilder<TScenario> When(Expression<Action<TScenario>> step, string stepTextTemplate);

        IFluentStepBuilder<TScenario> When(Expression<Action<TScenario>> step, bool includeInputsInStepTitle);

        IFluentStepBuilder<TScenario> When(Expression<Action<TScenario>> step);

        IFluentStepBuilder<TScenario> When(Expression<Func<TScenario, Task>> step, string stepTextTemplate);

        IFluentStepBuilder<TScenario> When(Expression<Func<TScenario, Task>> step, bool includeInputsInStepTitle);

        IFluentStepBuilder<TScenario> When(Expression<Func<TScenario, Task>> step);

        IFluentStepBuilder<TScenario> When(Action step, string title);

        IFluentStepBuilder<TScenario> When(Func<Task> step, string title);

        IFluentStepBuilder<TScenario> When(string title);

        IFluentStepBuilder<TScenario> Then(Expression<Action<TScenario>> step, string stepTextTemplate);

        IFluentStepBuilder<TScenario> Then(Expression<Action<TScenario>> step, bool includeInputsInStepTitle);

        IFluentStepBuilder<TScenario> Then(Expression<Action<TScenario>> step);

        IFluentStepBuilder<TScenario> Then(Expression<Func<TScenario, Task>> step, string stepTextTemplate);

        IFluentStepBuilder<TScenario> Then(Expression<Func<TScenario, Task>> step, bool includeInputsInStepTitle);

        IFluentStepBuilder<TScenario> Then(Expression<Func<TScenario, Task>> step);

        IFluentStepBuilder<TScenario> Then(Action step, string title);

        IFluentStepBuilder<TScenario> Then(Func<Task> step, string title);

        IFluentStepBuilder<TScenario> Then(string title);

        IFluentStepBuilder<TScenario> And(Expression<Action<TScenario>> step, string stepTextTemplate);

        IFluentStepBuilder<TScenario> And(Expression<Action<TScenario>> step, bool includeInputsInStepTitle);

        IFluentStepBuilder<TScenario> And(Expression<Action<TScenario>> step);

        IFluentStepBuilder<TScenario> And(Expression<Func<TScenario, Task>> step, string stepTextTemplate);

        IFluentStepBuilder<TScenario> And(Expression<Func<TScenario, Task>> step, bool includeInputsInStepTitle);

        IFluentStepBuilder<TScenario> And(Expression<Func<TScenario, Task>> step);

        IFluentStepBuilder<TScenario> And(Action step, string title);

        IFluentStepBuilder<TScenario> And(Func<Task> step, string title);

        IFluentStepBuilder<TScenario> And(string title);

        IFluentStepBuilder<TScenario> But(Expression<Action<TScenario>> step, string stepTextTemplate);

        IFluentStepBuilder<TScenario> But(Expression<Action<TScenario>> step, bool includeInputsInStepTitle);

        IFluentStepBuilder<TScenario> But(Expression<Action<TScenario>> step);

        IFluentStepBuilder<TScenario> But(Expression<Func<TScenario, Task>> step, string stepTextTemplate);

        IFluentStepBuilder<TScenario> But(Expression<Func<TScenario, Task>> step, bool includeInputsInStepTitle);

        IFluentStepBuilder<TScenario> But(Expression<Func<TScenario, Task>> step);

        IFluentStepBuilder<TScenario> But(Action step, string title);

        IFluentStepBuilder<TScenario> But(Func<Task> step, string title);

        IFluentStepBuilder<TScenario> But(string title);

        IFluentStepBuilder<TScenario> TearDownWith(Expression<Action<TScenario>> step, string stepTextTemplate);

        IFluentStepBuilder<TScenario> TearDownWith(Expression<Action<TScenario>> step, bool includeInputsInStepTitle);

        IFluentStepBuilder<TScenario> TearDownWith(Expression<Action<TScenario>> step);

        IFluentStepBuilder<TScenario> TearDownWith(Expression<Func<TScenario, Task>> step, string stepTextTemplate);

        IFluentStepBuilder<TScenario> TearDownWith(Expression<Func<TScenario, Task>> step, bool includeInputsInStepTitle);

        IFluentStepBuilder<TScenario> TearDownWith(Expression<Func<TScenario, Task>> step);

        IFluentStepBuilder<TScenario> TearDownWith(Action step, string title);

        IFluentStepBuilder<TScenario> TearDownWith(Func<Task> step, string title);

        IFluentStepBuilder<TScenario> TearDownWith(string title);
    }

    interface IFluentStepBuilder
    {
        object TestObject { get; }
    }

    public class FluentStepBuilder<TScenario> : IFluentStepBuilder<TScenario>, IFluentStepBuilder 
                                                where TScenario : class
    {
        readonly FluentScanner<TScenario> scanner;

        public FluentStepBuilder(TScenario testObject)
        {
            TestObject = testObject;
            var existingContext = TestContext.GetContext(TestObject);
            if (existingContext.FluentScanner == null)
                existingContext.FluentScanner = new FluentScanner<TScenario>(testObject);
 
            scanner = (FluentScanner<TScenario>) existingContext.FluentScanner;
        }

        public TScenario TestObject { get; private set; }

        object IFluentStepBuilder.TestObject { get { return TestObject; } }

        public IFluentStepBuilder<TScenario> Given(Expression<Action<TScenario>> step, string stepTextTemplate)
        {
            scanner.AddStep(step, stepTextTemplate, true, true, ExecutionOrder.SetupState, false, "Given");
            return this;
        }

        public IFluentStepBuilder<TScenario> Given(Expression<Action<TScenario>> step, bool includeInputsInStepTitle)
        {
            scanner.AddStep(step, null, includeInputsInStepTitle, true, ExecutionOrder.SetupState, false, "Given");
            return this;
        }

        public IFluentStepBuilder<TScenario> Given(Expression<Action<TScenario>> step)
        {
            scanner.AddStep(step, null, true, true, ExecutionOrder.SetupState, false, "Given");
            return this;
        }

        public IFluentStepBuilder<TScenario> Given(Expression<Func<TScenario, Task>> step, string stepTextTemplate)
        {
            scanner.AddStep(step, stepTextTemplate, true, true, ExecutionOrder.SetupState, false, "Given");
            return this;
        }

        public IFluentStepBuilder<TScenario> Given(Expression<Func<TScenario, Task>> step, bool includeInputsInStepTitle)
        {
            scanner.AddStep(step, null, includeInputsInStepTitle, true, ExecutionOrder.SetupState, false, "Given");
            return this;
        }

        public IFluentStepBuilder<TScenario> Given(Expression<Func<TScenario, Task>> step)
        {
            scanner.AddStep(step, null, true, true, ExecutionOrder.SetupState, false, "Given");
            return this;
        }

        public IFluentStepBuilder<TScenario> Given(Action step, string title)
        {
            scanner.AddStep(step, title, true, ExecutionOrder.SetupState, false, "Given");
            return this;
        }

        public IFluentStepBuilder<TScenario> Given(Func<Task> step, string title)
        {
            scanner.AddStep(step, title, true, ExecutionOrder.SetupState, false, "Given");
            return this;
        }

        public IFluentStepBuilder<TScenario> Given(string title)
        {
            scanner.AddStep(() => { }, title, true, ExecutionOrder.SetupState, false, "Given");
            return this;
        }
        public IFluentStepBuilder<TScenario> When(Expression<Action<TScenario>> step, string stepTextTemplate)
        {
            scanner.AddStep(step, stepTextTemplate, true, true, ExecutionOrder.Transition, false, "When");
            return this;
        }

        public IFluentStepBuilder<TScenario> When(Expression<Action<TScenario>> step, bool includeInputsInStepTitle)
        {
            scanner.AddStep(step, null, includeInputsInStepTitle, true, ExecutionOrder.Transition, false, "When");
            return this;
        }

        public IFluentStepBuilder<TScenario> When(Expression<Action<TScenario>> step)
        {
            scanner.AddStep(step, null, true, true, ExecutionOrder.Transition, false, "When");
            return this;
        }

        public IFluentStepBuilder<TScenario> When(Expression<Func<TScenario, Task>> step, string stepTextTemplate)
        {
            scanner.AddStep(step, stepTextTemplate, true, true, ExecutionOrder.Transition, false, "When");
            return this;
        }

        public IFluentStepBuilder<TScenario> When(Expression<Func<TScenario, Task>> step, bool includeInputsInStepTitle)
        {
            scanner.AddStep(step, null, includeInputsInStepTitle, true, ExecutionOrder.Transition, false, "When");
            return this;
        }

        public IFluentStepBuilder<TScenario> When(Expression<Func<TScenario, Task>> step)
        {
            scanner.AddStep(step, null, true, true, ExecutionOrder.Transition, false, "When");
            return this;
        }

        public IFluentStepBuilder<TScenario> When(Action step, string title)
        {
            scanner.AddStep(step, title, true, ExecutionOrder.Transition, false, "When");
            return this;
        }

        public IFluentStepBuilder<TScenario> When(Func<Task> step, string title)
        {
            scanner.AddStep(step, title, true, ExecutionOrder.Transition, false, "When");
            return this;
        }

        public IFluentStepBuilder<TScenario> When(string title)
        {
            scanner.AddStep(() => { }, title, true, ExecutionOrder.Transition, false, "When");
            return this;
        }
        public IFluentStepBuilder<TScenario> Then(Expression<Action<TScenario>> step, string stepTextTemplate)
        {
            scanner.AddStep(step, stepTextTemplate, true, true, ExecutionOrder.Assertion, true, "Then");
            return this;
        }

        public IFluentStepBuilder<TScenario> Then(Expression<Action<TScenario>> step, bool includeInputsInStepTitle)
        {
            scanner.AddStep(step, null, includeInputsInStepTitle, true, ExecutionOrder.Assertion, true, "Then");
            return this;
        }

        public IFluentStepBuilder<TScenario> Then(Expression<Action<TScenario>> step)
        {
            scanner.AddStep(step, null, true, true, ExecutionOrder.Assertion, true, "Then");
            return this;
        }

        public IFluentStepBuilder<TScenario> Then(Expression<Func<TScenario, Task>> step, string stepTextTemplate)
        {
            scanner.AddStep(step, stepTextTemplate, true, true, ExecutionOrder.Assertion, true, "Then");
            return this;
        }

        public IFluentStepBuilder<TScenario> Then(Expression<Func<TScenario, Task>> step, bool includeInputsInStepTitle)
        {
            scanner.AddStep(step, null, includeInputsInStepTitle, true, ExecutionOrder.Assertion, true, "Then");
            return this;
        }

        public IFluentStepBuilder<TScenario> Then(Expression<Func<TScenario, Task>> step)
        {
            scanner.AddStep(step, null, true, true, ExecutionOrder.Assertion, true, "Then");
            return this;
        }

        public IFluentStepBuilder<TScenario> Then(Action step, string title)
        {
            scanner.AddStep(step, title, true, ExecutionOrder.Assertion, true, "Then");
            return this;
        }

        public IFluentStepBuilder<TScenario> Then(Func<Task> step, string title)
        {
            scanner.AddStep(step, title, true, ExecutionOrder.Assertion, true, "Then");
            return this;
        }

        public IFluentStepBuilder<TScenario> Then(string title)
        {
            scanner.AddStep(() => { }, title, true, ExecutionOrder.Assertion, true, "Then");
            return this;
        }
        public IFluentStepBuilder<TScenario> And(Expression<Action<TScenario>> step, string stepTextTemplate)
        {
            scanner.AddStep(step, stepTextTemplate, true, true, ExecutionOrder.ConsecutiveStep, false, "And");
            return this;
        }

        public IFluentStepBuilder<TScenario> And(Expression<Action<TScenario>> step, bool includeInputsInStepTitle)
        {
            scanner.AddStep(step, null, includeInputsInStepTitle, true, ExecutionOrder.ConsecutiveStep, false, "And");
            return this;
        }

        public IFluentStepBuilder<TScenario> And(Expression<Action<TScenario>> step)
        {
            scanner.AddStep(step, null, true, true, ExecutionOrder.ConsecutiveStep, false, "And");
            return this;
        }

        public IFluentStepBuilder<TScenario> And(Expression<Func<TScenario, Task>> step, string stepTextTemplate)
        {
            scanner.AddStep(step, stepTextTemplate, true, true, ExecutionOrder.ConsecutiveStep, false, "And");
            return this;
        }

        public IFluentStepBuilder<TScenario> And(Expression<Func<TScenario, Task>> step, bool includeInputsInStepTitle)
        {
            scanner.AddStep(step, null, includeInputsInStepTitle, true, ExecutionOrder.ConsecutiveStep, false, "And");
            return this;
        }

        public IFluentStepBuilder<TScenario> And(Expression<Func<TScenario, Task>> step)
        {
            scanner.AddStep(step, null, true, true, ExecutionOrder.ConsecutiveStep, false, "And");
            return this;
        }

        public IFluentStepBuilder<TScenario> And(Action step, string title)
        {
            scanner.AddStep(step, title, true, ExecutionOrder.ConsecutiveStep, false, "And");
            return this;
        }

        public IFluentStepBuilder<TScenario> And(Func<Task> step, string title)
        {
            scanner.AddStep(step, title, true, ExecutionOrder.ConsecutiveStep, false, "And");
            return this;
        }

        public IFluentStepBuilder<TScenario> And(string title)
        {
            scanner.AddStep(() => { }, title, true, ExecutionOrder.ConsecutiveStep, false, "And");
            return this;
        }
        public IFluentStepBuilder<TScenario> But(Expression<Action<TScenario>> step, string stepTextTemplate)
        {
            scanner.AddStep(step, stepTextTemplate, true, true, ExecutionOrder.ConsecutiveStep, false, "But");
            return this;
        }

        public IFluentStepBuilder<TScenario> But(Expression<Action<TScenario>> step, bool includeInputsInStepTitle)
        {
            scanner.AddStep(step, null, includeInputsInStepTitle, true, ExecutionOrder.ConsecutiveStep, false, "But");
            return this;
        }

        public IFluentStepBuilder<TScenario> But(Expression<Action<TScenario>> step)
        {
            scanner.AddStep(step, null, true, true, ExecutionOrder.ConsecutiveStep, false, "But");
            return this;
        }

        public IFluentStepBuilder<TScenario> But(Expression<Func<TScenario, Task>> step, string stepTextTemplate)
        {
            scanner.AddStep(step, stepTextTemplate, true, true, ExecutionOrder.ConsecutiveStep, false, "But");
            return this;
        }

        public IFluentStepBuilder<TScenario> But(Expression<Func<TScenario, Task>> step, bool includeInputsInStepTitle)
        {
            scanner.AddStep(step, null, includeInputsInStepTitle, true, ExecutionOrder.ConsecutiveStep, false, "But");
            return this;
        }

        public IFluentStepBuilder<TScenario> But(Expression<Func<TScenario, Task>> step)
        {
            scanner.AddStep(step, null, true, true, ExecutionOrder.ConsecutiveStep, false, "But");
            return this;
        }

        public IFluentStepBuilder<TScenario> But(Action step, string title)
        {
            scanner.AddStep(step, title, true, ExecutionOrder.ConsecutiveStep, false, "But");
            return this;
        }

        public IFluentStepBuilder<TScenario> But(Func<Task> step, string title)
        {
            scanner.AddStep(step, title, true, ExecutionOrder.ConsecutiveStep, false, "But");
            return this;
        }

        public IFluentStepBuilder<TScenario> But(string title)
        {
            scanner.AddStep(() => { }, title, true, ExecutionOrder.ConsecutiveStep, false, "But");
            return this;
        }
        public IFluentStepBuilder<TScenario> TearDownWith(Expression<Action<TScenario>> step, string stepTextTemplate)
        {
            scanner.AddStep(step, stepTextTemplate, true, false, ExecutionOrder.TearDown, false, "");
            return this;
        }

        public IFluentStepBuilder<TScenario> TearDownWith(Expression<Action<TScenario>> step, bool includeInputsInStepTitle)
        {
            scanner.AddStep(step, null, includeInputsInStepTitle, false, ExecutionOrder.TearDown, false, "");
            return this;
        }

        public IFluentStepBuilder<TScenario> TearDownWith(Expression<Action<TScenario>> step)
        {
            scanner.AddStep(step, null, true, false, ExecutionOrder.TearDown, false, "");
            return this;
        }

        public IFluentStepBuilder<TScenario> TearDownWith(Expression<Func<TScenario, Task>> step, string stepTextTemplate)
        {
            scanner.AddStep(step, stepTextTemplate, true, false, ExecutionOrder.TearDown, false, "");
            return this;
        }

        public IFluentStepBuilder<TScenario> TearDownWith(Expression<Func<TScenario, Task>> step, bool includeInputsInStepTitle)
        {
            scanner.AddStep(step, null, includeInputsInStepTitle, false, ExecutionOrder.TearDown, false, "");
            return this;
        }

        public IFluentStepBuilder<TScenario> TearDownWith(Expression<Func<TScenario, Task>> step)
        {
            scanner.AddStep(step, null, true, false, ExecutionOrder.TearDown, false, "");
            return this;
        }

        public IFluentStepBuilder<TScenario> TearDownWith(Action step, string title)
        {
            scanner.AddStep(step, title, false, ExecutionOrder.TearDown, false, "");
            return this;
        }

        public IFluentStepBuilder<TScenario> TearDownWith(Func<Task> step, string title)
        {
            scanner.AddStep(step, title, false, ExecutionOrder.TearDown, false, "");
            return this;
        }

        public IFluentStepBuilder<TScenario> TearDownWith(string title)
        {
            scanner.AddStep(() => { }, title, false, ExecutionOrder.TearDown, false, "");
            return this;
        }
    }
}