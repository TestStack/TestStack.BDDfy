using System;
using System.Reflection;
using System.Threading.Tasks;
using Shouldly;
using TestStack.BDDfy.Processors;
using Xunit;

namespace TestStack.BDDfy.Tests.Scanner.ReflectiveScanner
{
    public class WhenStepScannerFactoryAsyncMethods
    {
        [Fact]
        public void CallingAsyncTaskWhichThrowsIsObservedAndRethrown()
        {
            var stepAction = StepActionFactory.GetStepAction<SomeScenario>(o => AsyncTaskMethod(o));            
            Should.Throw<ArgumentException>(()=> AsyncTestRunner.Run(() => stepAction(new SomeScenario())));
        }

        [Fact]
        public void CallingAsyncVoidWhichThrowsIsObservedAndRethrown()
        {
            var stepAction = StepActionFactory.GetStepAction<SomeScenario>(s=>AsyncVoidMethod(s));

            Should.Throw<ArgumentException>(()=> AsyncTestRunner.Run(() => stepAction(new SomeScenario())));
        }

        [Fact]
        public void InvokingAsyncTaskWhichThrowsIsObservedAndRethrown()
        {
            var methodInfo = typeof(WhenStepScannerFactoryAsyncMethods).GetMethod("AsyncVoidMethod", BindingFlags.Instance | BindingFlags.NonPublic);
            var stepAction = StepActionFactory.GetStepAction(methodInfo, new object[] { new SomeScenario() });

            Should.Throw<ArgumentException>(()=> AsyncTestRunner.Run(() => stepAction(this)));
        }

        [Fact]
        public void InvokingAsyncVoidWhichThrowsIsObservedAndRethrown()
        {
            var methodInfo = typeof(WhenStepScannerFactoryAsyncMethods).GetMethod("AsyncTaskMethod", BindingFlags.Instance | BindingFlags.NonPublic);
            var stepAction = StepActionFactory.GetStepAction(methodInfo, new object[] { new SomeScenario() });

            Should.Throw<ArgumentException>(()=> AsyncTestRunner.Run(() => stepAction(this)));
        }

        private async void AsyncVoidMethod(SomeScenario someScenario)
        {
            await Task.Yield();
            throw new ArgumentException();
        }

        private async Task AsyncTaskMethod(SomeScenario obj)
        {
            await Task.Yield();
            throw new ArgumentException();
        }

        private class SomeScenario
        {
        }
    }
}