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
        static FluentScanner<TScenario> Scan<TScenario>(this TScenario testObject) where TScenario : class
        {
            var existingContext = TestContext.GetContext(testObject);
            if (existingContext.FluentScanner == null)
                existingContext.FluentScanner = new FluentScanner<TScenario>(testObject);
 
            return (FluentScanner<TScenario>) existingContext.FluentScanner;
        }
 
        public static IFluentTestBuilder<TScenario> Given<TScenario>(this TScenario testObject, Expression<Action<TScenario>> step, string stepTextTemplate)
            where TScenario: class
        {
            testObject.Scan().AddStep(step, stepTextTemplate, true, true, ExecutionOrder.SetupState, false);
            return new FluentTestBuilder<TScenario>(testObject);
        }
 
        public static IFluentTestBuilder<TScenario> Given<TScenario>(this TScenario testObject, Expression<Action<TScenario>> step, bool includeInputsInStepTitle)
            where TScenario: class
        {
            testObject.Scan().AddStep(step, null, includeInputsInStepTitle, true, ExecutionOrder.SetupState, false);
            return new FluentTestBuilder<TScenario>(testObject);
        }
 
        public static IFluentTestBuilder<TScenario> Given<TScenario>(this TScenario testObject, Expression<Action<TScenario>> step)
            where TScenario: class
        {
            testObject.Scan().AddStep(step, null, true, true, ExecutionOrder.SetupState, false);
            return new FluentTestBuilder<TScenario>(testObject);
        }
        
        public static IFluentTestBuilder<TScenario> Given<TScenario>(this TScenario testObject, Expression<Func<TScenario, Task>> step, string stepTextTemplate)
            where TScenario: class
        {
            testObject.Scan().AddStep(step, stepTextTemplate, true, true, ExecutionOrder.SetupState, false);
            return new FluentTestBuilder<TScenario>(testObject);
        }
 
        public static IFluentTestBuilder<TScenario> Given<TScenario>(this TScenario testObject, Expression<Func<TScenario, Task>> step, bool includeInputsInStepTitle)
            where TScenario: class
        {
            testObject.Scan().AddStep(step, null, includeInputsInStepTitle, true, ExecutionOrder.SetupState, false);
            return new FluentTestBuilder<TScenario>(testObject);
        }
 
        public static IFluentTestBuilder<TScenario> Given<TScenario>(this TScenario testObject, Expression<Func<TScenario, Task>> step)
            where TScenario: class
        {
            testObject.Scan().AddStep(step, null, true, true, ExecutionOrder.SetupState, false);
            return new FluentTestBuilder<TScenario>(testObject);
        }
 
        public static IFluentTestBuilder<TScenario> Given<TScenario>(this TScenario testObject, Action step, string title)
            where TScenario : class
        {
            testObject.Scan().AddStep(step, title, true, ExecutionOrder.SetupState, false);
            return new FluentTestBuilder<TScenario>(testObject);
        }
 
        public static IFluentTestBuilder<TScenario> Given<TScenario>(this TScenario testObject, Func<Task> step, string title)
            where TScenario : class
        {
            testObject.Scan().AddStep(step, title, true, ExecutionOrder.SetupState, false);
            return new FluentTestBuilder<TScenario>(testObject);
        }
 
        public static IFluentTestBuilder<TScenario> Given<TScenario>(this TScenario testObject, string title)
            where TScenario : class
        {
            testObject.Scan().AddStep(() => { }, title, true, ExecutionOrder.SetupState, false);
            return new FluentTestBuilder<TScenario>(testObject);
        }
        public static IFluentTestBuilder<TScenario> Given<TScenario>(this IFluentTestBuilder<TScenario> testObject, Expression<Action<TScenario>> step, string stepTextTemplate)
            where TScenario: class
        {
            testObject.TestObject.Scan().AddStep(step, stepTextTemplate, true, true, ExecutionOrder.SetupState, false);
            return new FluentTestBuilder<TScenario>(testObject.TestObject);
        }
 
        public static IFluentTestBuilder<TScenario> Given<TScenario>(this IFluentTestBuilder<TScenario> testObject, Expression<Action<TScenario>> step, bool includeInputsInStepTitle)
            where TScenario: class
        {
            testObject.TestObject.Scan().AddStep(step, null, includeInputsInStepTitle, true, ExecutionOrder.SetupState, false);
            return new FluentTestBuilder<TScenario>(testObject.TestObject);
        }
 
        public static IFluentTestBuilder<TScenario> Given<TScenario>(this IFluentTestBuilder<TScenario> testObject, Expression<Action<TScenario>> step)
            where TScenario: class
        {
            testObject.TestObject.Scan().AddStep(step, null, true, true, ExecutionOrder.SetupState, false);
            return new FluentTestBuilder<TScenario>(testObject.TestObject);
        }
        
        public static IFluentTestBuilder<TScenario> Given<TScenario>(this IFluentTestBuilder<TScenario> testObject, Expression<Func<TScenario, Task>> step, string stepTextTemplate)
            where TScenario: class
        {
            testObject.TestObject.Scan().AddStep(step, stepTextTemplate, true, true, ExecutionOrder.SetupState, false);
            return new FluentTestBuilder<TScenario>(testObject.TestObject);
        }
 
        public static IFluentTestBuilder<TScenario> Given<TScenario>(this IFluentTestBuilder<TScenario> testObject, Expression<Func<TScenario, Task>> step, bool includeInputsInStepTitle)
            where TScenario: class
        {
            testObject.TestObject.Scan().AddStep(step, null, includeInputsInStepTitle, true, ExecutionOrder.SetupState, false);
            return new FluentTestBuilder<TScenario>(testObject.TestObject);
        }
 
        public static IFluentTestBuilder<TScenario> Given<TScenario>(this IFluentTestBuilder<TScenario> testObject, Expression<Func<TScenario, Task>> step)
            where TScenario: class
        {
            testObject.TestObject.Scan().AddStep(step, null, true, true, ExecutionOrder.SetupState, false);
            return new FluentTestBuilder<TScenario>(testObject.TestObject);
        }
 
        public static IFluentTestBuilder<TScenario> Given<TScenario>(this IFluentTestBuilder<TScenario> testObject, Action step, string title)
            where TScenario : class
        {
            testObject.TestObject.Scan().AddStep(step, title, true, ExecutionOrder.SetupState, false);
            return new FluentTestBuilder<TScenario>(testObject.TestObject);
        }
 
        public static IFluentTestBuilder<TScenario> Given<TScenario>(this IFluentTestBuilder<TScenario> testObject, Func<Task> step, string title)
            where TScenario : class
        {
            testObject.TestObject.Scan().AddStep(step, title, true, ExecutionOrder.SetupState, false);
            return new FluentTestBuilder<TScenario>(testObject.TestObject);
        }
 
        public static IFluentTestBuilder<TScenario> Given<TScenario>(this IFluentTestBuilder<TScenario> testObject, string title)
            where TScenario : class
        {
            testObject.TestObject.Scan().AddStep(() => { }, title, true, ExecutionOrder.SetupState, false);
            return new FluentTestBuilder<TScenario>(testObject.TestObject);
        }
        public static IFluentTestBuilder<TScenario> When<TScenario>(this IFluentTestBuilder<TScenario> testObject, Expression<Action<TScenario>> step, string stepTextTemplate)
            where TScenario: class
        {
            testObject.TestObject.Scan().AddStep(step, stepTextTemplate, true, true, ExecutionOrder.Transition, false);
            return new FluentTestBuilder<TScenario>(testObject.TestObject);
        }
 
        public static IFluentTestBuilder<TScenario> When<TScenario>(this IFluentTestBuilder<TScenario> testObject, Expression<Action<TScenario>> step, bool includeInputsInStepTitle)
            where TScenario: class
        {
            testObject.TestObject.Scan().AddStep(step, null, includeInputsInStepTitle, true, ExecutionOrder.Transition, false);
            return new FluentTestBuilder<TScenario>(testObject.TestObject);
        }
 
        public static IFluentTestBuilder<TScenario> When<TScenario>(this IFluentTestBuilder<TScenario> testObject, Expression<Action<TScenario>> step)
            where TScenario: class
        {
            testObject.TestObject.Scan().AddStep(step, null, true, true, ExecutionOrder.Transition, false);
            return new FluentTestBuilder<TScenario>(testObject.TestObject);
        }
        
        public static IFluentTestBuilder<TScenario> When<TScenario>(this IFluentTestBuilder<TScenario> testObject, Expression<Func<TScenario, Task>> step, string stepTextTemplate)
            where TScenario: class
        {
            testObject.TestObject.Scan().AddStep(step, stepTextTemplate, true, true, ExecutionOrder.Transition, false);
            return new FluentTestBuilder<TScenario>(testObject.TestObject);
        }
 
        public static IFluentTestBuilder<TScenario> When<TScenario>(this IFluentTestBuilder<TScenario> testObject, Expression<Func<TScenario, Task>> step, bool includeInputsInStepTitle)
            where TScenario: class
        {
            testObject.TestObject.Scan().AddStep(step, null, includeInputsInStepTitle, true, ExecutionOrder.Transition, false);
            return new FluentTestBuilder<TScenario>(testObject.TestObject);
        }
 
        public static IFluentTestBuilder<TScenario> When<TScenario>(this IFluentTestBuilder<TScenario> testObject, Expression<Func<TScenario, Task>> step)
            where TScenario: class
        {
            testObject.TestObject.Scan().AddStep(step, null, true, true, ExecutionOrder.Transition, false);
            return new FluentTestBuilder<TScenario>(testObject.TestObject);
        }
 
        public static IFluentTestBuilder<TScenario> When<TScenario>(this IFluentTestBuilder<TScenario> testObject, Action step, string title)
            where TScenario : class
        {
            testObject.TestObject.Scan().AddStep(step, title, true, ExecutionOrder.Transition, false);
            return new FluentTestBuilder<TScenario>(testObject.TestObject);
        }
 
        public static IFluentTestBuilder<TScenario> When<TScenario>(this IFluentTestBuilder<TScenario> testObject, Func<Task> step, string title)
            where TScenario : class
        {
            testObject.TestObject.Scan().AddStep(step, title, true, ExecutionOrder.Transition, false);
            return new FluentTestBuilder<TScenario>(testObject.TestObject);
        }
 
        public static IFluentTestBuilder<TScenario> When<TScenario>(this IFluentTestBuilder<TScenario> testObject, string title)
            where TScenario : class
        {
            testObject.TestObject.Scan().AddStep(() => { }, title, true, ExecutionOrder.Transition, false);
            return new FluentTestBuilder<TScenario>(testObject.TestObject);
        }
        public static IFluentTestBuilder<TScenario> Then<TScenario>(this IFluentTestBuilder<TScenario> testObject, Expression<Action<TScenario>> step, string stepTextTemplate)
            where TScenario: class
        {
            testObject.TestObject.Scan().AddStep(step, stepTextTemplate, true, true, ExecutionOrder.Assertion, true);
            return new FluentTestBuilder<TScenario>(testObject.TestObject);
        }
 
        public static IFluentTestBuilder<TScenario> Then<TScenario>(this IFluentTestBuilder<TScenario> testObject, Expression<Action<TScenario>> step, bool includeInputsInStepTitle)
            where TScenario: class
        {
            testObject.TestObject.Scan().AddStep(step, null, includeInputsInStepTitle, true, ExecutionOrder.Assertion, true);
            return new FluentTestBuilder<TScenario>(testObject.TestObject);
        }
 
        public static IFluentTestBuilder<TScenario> Then<TScenario>(this IFluentTestBuilder<TScenario> testObject, Expression<Action<TScenario>> step)
            where TScenario: class
        {
            testObject.TestObject.Scan().AddStep(step, null, true, true, ExecutionOrder.Assertion, true);
            return new FluentTestBuilder<TScenario>(testObject.TestObject);
        }
        
        public static IFluentTestBuilder<TScenario> Then<TScenario>(this IFluentTestBuilder<TScenario> testObject, Expression<Func<TScenario, Task>> step, string stepTextTemplate)
            where TScenario: class
        {
            testObject.TestObject.Scan().AddStep(step, stepTextTemplate, true, true, ExecutionOrder.Assertion, true);
            return new FluentTestBuilder<TScenario>(testObject.TestObject);
        }
 
        public static IFluentTestBuilder<TScenario> Then<TScenario>(this IFluentTestBuilder<TScenario> testObject, Expression<Func<TScenario, Task>> step, bool includeInputsInStepTitle)
            where TScenario: class
        {
            testObject.TestObject.Scan().AddStep(step, null, includeInputsInStepTitle, true, ExecutionOrder.Assertion, true);
            return new FluentTestBuilder<TScenario>(testObject.TestObject);
        }
 
        public static IFluentTestBuilder<TScenario> Then<TScenario>(this IFluentTestBuilder<TScenario> testObject, Expression<Func<TScenario, Task>> step)
            where TScenario: class
        {
            testObject.TestObject.Scan().AddStep(step, null, true, true, ExecutionOrder.Assertion, true);
            return new FluentTestBuilder<TScenario>(testObject.TestObject);
        }
 
        public static IFluentTestBuilder<TScenario> Then<TScenario>(this IFluentTestBuilder<TScenario> testObject, Action step, string title)
            where TScenario : class
        {
            testObject.TestObject.Scan().AddStep(step, title, true, ExecutionOrder.Assertion, true);
            return new FluentTestBuilder<TScenario>(testObject.TestObject);
        }
 
        public static IFluentTestBuilder<TScenario> Then<TScenario>(this IFluentTestBuilder<TScenario> testObject, Func<Task> step, string title)
            where TScenario : class
        {
            testObject.TestObject.Scan().AddStep(step, title, true, ExecutionOrder.Assertion, true);
            return new FluentTestBuilder<TScenario>(testObject.TestObject);
        }
 
        public static IFluentTestBuilder<TScenario> Then<TScenario>(this IFluentTestBuilder<TScenario> testObject, string title)
            where TScenario : class
        {
            testObject.TestObject.Scan().AddStep(() => { }, title, true, ExecutionOrder.Assertion, true);
            return new FluentTestBuilder<TScenario>(testObject.TestObject);
        }
        public static IFluentTestBuilder<TScenario> And<TScenario>(this IFluentTestBuilder<TScenario> testObject, Expression<Action<TScenario>> step, string stepTextTemplate)
            where TScenario: class
        {
            testObject.TestObject.Scan().AddStep(step, stepTextTemplate, true, true, ExecutionOrder.ConsecutiveStep, false);
            return new FluentTestBuilder<TScenario>(testObject.TestObject);
        }
 
        public static IFluentTestBuilder<TScenario> And<TScenario>(this IFluentTestBuilder<TScenario> testObject, Expression<Action<TScenario>> step, bool includeInputsInStepTitle)
            where TScenario: class
        {
            testObject.TestObject.Scan().AddStep(step, null, includeInputsInStepTitle, true, ExecutionOrder.ConsecutiveStep, false);
            return new FluentTestBuilder<TScenario>(testObject.TestObject);
        }
 
        public static IFluentTestBuilder<TScenario> And<TScenario>(this IFluentTestBuilder<TScenario> testObject, Expression<Action<TScenario>> step)
            where TScenario: class
        {
            testObject.TestObject.Scan().AddStep(step, null, true, true, ExecutionOrder.ConsecutiveStep, false);
            return new FluentTestBuilder<TScenario>(testObject.TestObject);
        }
        
        public static IFluentTestBuilder<TScenario> And<TScenario>(this IFluentTestBuilder<TScenario> testObject, Expression<Func<TScenario, Task>> step, string stepTextTemplate)
            where TScenario: class
        {
            testObject.TestObject.Scan().AddStep(step, stepTextTemplate, true, true, ExecutionOrder.ConsecutiveStep, false);
            return new FluentTestBuilder<TScenario>(testObject.TestObject);
        }
 
        public static IFluentTestBuilder<TScenario> And<TScenario>(this IFluentTestBuilder<TScenario> testObject, Expression<Func<TScenario, Task>> step, bool includeInputsInStepTitle)
            where TScenario: class
        {
            testObject.TestObject.Scan().AddStep(step, null, includeInputsInStepTitle, true, ExecutionOrder.ConsecutiveStep, false);
            return new FluentTestBuilder<TScenario>(testObject.TestObject);
        }
 
        public static IFluentTestBuilder<TScenario> And<TScenario>(this IFluentTestBuilder<TScenario> testObject, Expression<Func<TScenario, Task>> step)
            where TScenario: class
        {
            testObject.TestObject.Scan().AddStep(step, null, true, true, ExecutionOrder.ConsecutiveStep, false);
            return new FluentTestBuilder<TScenario>(testObject.TestObject);
        }
 
        public static IFluentTestBuilder<TScenario> And<TScenario>(this IFluentTestBuilder<TScenario> testObject, Action step, string title)
            where TScenario : class
        {
            testObject.TestObject.Scan().AddStep(step, title, true, ExecutionOrder.ConsecutiveStep, false);
            return new FluentTestBuilder<TScenario>(testObject.TestObject);
        }
 
        public static IFluentTestBuilder<TScenario> And<TScenario>(this IFluentTestBuilder<TScenario> testObject, Func<Task> step, string title)
            where TScenario : class
        {
            testObject.TestObject.Scan().AddStep(step, title, true, ExecutionOrder.ConsecutiveStep, false);
            return new FluentTestBuilder<TScenario>(testObject.TestObject);
        }
 
        public static IFluentTestBuilder<TScenario> And<TScenario>(this IFluentTestBuilder<TScenario> testObject, string title)
            where TScenario : class
        {
            testObject.TestObject.Scan().AddStep(() => { }, title, true, ExecutionOrder.ConsecutiveStep, false);
            return new FluentTestBuilder<TScenario>(testObject.TestObject);
        }
        public static IFluentTestBuilder<TScenario> But<TScenario>(this IFluentTestBuilder<TScenario> testObject, Expression<Action<TScenario>> step, string stepTextTemplate)
            where TScenario: class
        {
            testObject.TestObject.Scan().AddStep(step, stepTextTemplate, true, true, ExecutionOrder.ConsecutiveStep, false);
            return new FluentTestBuilder<TScenario>(testObject.TestObject);
        }
 
        public static IFluentTestBuilder<TScenario> But<TScenario>(this IFluentTestBuilder<TScenario> testObject, Expression<Action<TScenario>> step, bool includeInputsInStepTitle)
            where TScenario: class
        {
            testObject.TestObject.Scan().AddStep(step, null, includeInputsInStepTitle, true, ExecutionOrder.ConsecutiveStep, false);
            return new FluentTestBuilder<TScenario>(testObject.TestObject);
        }
 
        public static IFluentTestBuilder<TScenario> But<TScenario>(this IFluentTestBuilder<TScenario> testObject, Expression<Action<TScenario>> step)
            where TScenario: class
        {
            testObject.TestObject.Scan().AddStep(step, null, true, true, ExecutionOrder.ConsecutiveStep, false);
            return new FluentTestBuilder<TScenario>(testObject.TestObject);
        }
        
        public static IFluentTestBuilder<TScenario> But<TScenario>(this IFluentTestBuilder<TScenario> testObject, Expression<Func<TScenario, Task>> step, string stepTextTemplate)
            where TScenario: class
        {
            testObject.TestObject.Scan().AddStep(step, stepTextTemplate, true, true, ExecutionOrder.ConsecutiveStep, false);
            return new FluentTestBuilder<TScenario>(testObject.TestObject);
        }
 
        public static IFluentTestBuilder<TScenario> But<TScenario>(this IFluentTestBuilder<TScenario> testObject, Expression<Func<TScenario, Task>> step, bool includeInputsInStepTitle)
            where TScenario: class
        {
            testObject.TestObject.Scan().AddStep(step, null, includeInputsInStepTitle, true, ExecutionOrder.ConsecutiveStep, false);
            return new FluentTestBuilder<TScenario>(testObject.TestObject);
        }
 
        public static IFluentTestBuilder<TScenario> But<TScenario>(this IFluentTestBuilder<TScenario> testObject, Expression<Func<TScenario, Task>> step)
            where TScenario: class
        {
            testObject.TestObject.Scan().AddStep(step, null, true, true, ExecutionOrder.ConsecutiveStep, false);
            return new FluentTestBuilder<TScenario>(testObject.TestObject);
        }
 
        public static IFluentTestBuilder<TScenario> But<TScenario>(this IFluentTestBuilder<TScenario> testObject, Action step, string title)
            where TScenario : class
        {
            testObject.TestObject.Scan().AddStep(step, title, true, ExecutionOrder.ConsecutiveStep, false);
            return new FluentTestBuilder<TScenario>(testObject.TestObject);
        }
 
        public static IFluentTestBuilder<TScenario> But<TScenario>(this IFluentTestBuilder<TScenario> testObject, Func<Task> step, string title)
            where TScenario : class
        {
            testObject.TestObject.Scan().AddStep(step, title, true, ExecutionOrder.ConsecutiveStep, false);
            return new FluentTestBuilder<TScenario>(testObject.TestObject);
        }
 
        public static IFluentTestBuilder<TScenario> But<TScenario>(this IFluentTestBuilder<TScenario> testObject, string title)
            where TScenario : class
        {
            testObject.TestObject.Scan().AddStep(() => { }, title, true, ExecutionOrder.ConsecutiveStep, false);
            return new FluentTestBuilder<TScenario>(testObject.TestObject);
        }
        public static IFluentTestBuilder<TScenario> TearDownWith<TScenario>(this IFluentTestBuilder<TScenario> testObject, Expression<Action<TScenario>> step, string stepTextTemplate)
            where TScenario: class
        {
            testObject.TestObject.Scan().AddStep(step, stepTextTemplate, true, false, ExecutionOrder.TearDown, false);
            return new FluentTestBuilder<TScenario>(testObject.TestObject);
        }
 
        public static IFluentTestBuilder<TScenario> TearDownWith<TScenario>(this IFluentTestBuilder<TScenario> testObject, Expression<Action<TScenario>> step, bool includeInputsInStepTitle)
            where TScenario: class
        {
            testObject.TestObject.Scan().AddStep(step, null, includeInputsInStepTitle, false, ExecutionOrder.TearDown, false);
            return new FluentTestBuilder<TScenario>(testObject.TestObject);
        }
 
        public static IFluentTestBuilder<TScenario> TearDownWith<TScenario>(this IFluentTestBuilder<TScenario> testObject, Expression<Action<TScenario>> step)
            where TScenario: class
        {
            testObject.TestObject.Scan().AddStep(step, null, true, false, ExecutionOrder.TearDown, false);
            return new FluentTestBuilder<TScenario>(testObject.TestObject);
        }
        
        public static IFluentTestBuilder<TScenario> TearDownWith<TScenario>(this IFluentTestBuilder<TScenario> testObject, Expression<Func<TScenario, Task>> step, string stepTextTemplate)
            where TScenario: class
        {
            testObject.TestObject.Scan().AddStep(step, stepTextTemplate, true, false, ExecutionOrder.TearDown, false);
            return new FluentTestBuilder<TScenario>(testObject.TestObject);
        }
 
        public static IFluentTestBuilder<TScenario> TearDownWith<TScenario>(this IFluentTestBuilder<TScenario> testObject, Expression<Func<TScenario, Task>> step, bool includeInputsInStepTitle)
            where TScenario: class
        {
            testObject.TestObject.Scan().AddStep(step, null, includeInputsInStepTitle, false, ExecutionOrder.TearDown, false);
            return new FluentTestBuilder<TScenario>(testObject.TestObject);
        }
 
        public static IFluentTestBuilder<TScenario> TearDownWith<TScenario>(this IFluentTestBuilder<TScenario> testObject, Expression<Func<TScenario, Task>> step)
            where TScenario: class
        {
            testObject.TestObject.Scan().AddStep(step, null, true, false, ExecutionOrder.TearDown, false);
            return new FluentTestBuilder<TScenario>(testObject.TestObject);
        }
 
        public static IFluentTestBuilder<TScenario> TearDownWith<TScenario>(this IFluentTestBuilder<TScenario> testObject, Action step, string title)
            where TScenario : class
        {
            testObject.TestObject.Scan().AddStep(step, title, false, ExecutionOrder.TearDown, false);
            return new FluentTestBuilder<TScenario>(testObject.TestObject);
        }
 
        public static IFluentTestBuilder<TScenario> TearDownWith<TScenario>(this IFluentTestBuilder<TScenario> testObject, Func<Task> step, string title)
            where TScenario : class
        {
            testObject.TestObject.Scan().AddStep(step, title, false, ExecutionOrder.TearDown, false);
            return new FluentTestBuilder<TScenario>(testObject.TestObject);
        }
 
        public static IFluentTestBuilder<TScenario> TearDownWith<TScenario>(this IFluentTestBuilder<TScenario> testObject, string title)
            where TScenario : class
        {
            testObject.TestObject.Scan().AddStep(() => { }, title, false, ExecutionOrder.TearDown, false);
            return new FluentTestBuilder<TScenario>(testObject.TestObject);
        }
    }
}