using System;
using System.Reflection;
using System.Threading.Tasks;
using NUnit.Framework;
using TestStack.BDDfy.Processors;

namespace TestStack.BDDfy.Tests.Scanner.ReflectiveScanner
{
    [TestFixture]
    public class WhenStepScannerFactoryAsyncMethods
    {
        [Test]
        public void CallingAsyncTaskWhichThrowsIsObservedAndRethrown()
        {
            var stepAction = StepActionFactory.GetStepAction<SomeScenario>(o => AsyncTaskMethod(o));            
            Assert.Throws<ArgumentException>(()=> AsyncTestRunner.Run(() => stepAction(new SomeScenario())));
        }

        [Test]
        public void CallingAsyncVoidWhichThrowsIsObservedAndRethrown()
        {
            var stepAction = StepActionFactory.GetStepAction<SomeScenario>(s=>AsyncVoidMethod(s));

            Assert.Throws<ArgumentException>(()=> AsyncTestRunner.Run(() => stepAction(new SomeScenario())));
        }

        [Test]
        public void InvokingAsyncTaskWhichThrowsIsObservedAndRethrown()
        {
            var methodInfo = typeof(WhenStepScannerFactoryAsyncMethods).GetMethod("AsyncVoidMethod", BindingFlags.Instance | BindingFlags.NonPublic);
            var stepAction = StepActionFactory.GetStepAction(methodInfo, new object[] { new SomeScenario() });

            Assert.Throws<ArgumentException>(()=> AsyncTestRunner.Run(() => stepAction(this)));
        }

        [Test]
        public void InvokingAsyncVoidWhichThrowsIsObservedAndRethrown()
        {
            var methodInfo = typeof(WhenStepScannerFactoryAsyncMethods).GetMethod("AsyncTaskMethod", BindingFlags.Instance | BindingFlags.NonPublic);
            var stepAction = StepActionFactory.GetStepAction(methodInfo, new object[] { new SomeScenario() });

            Assert.Throws<ArgumentException>(()=> AsyncTestRunner.Run(() => stepAction(this)));
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