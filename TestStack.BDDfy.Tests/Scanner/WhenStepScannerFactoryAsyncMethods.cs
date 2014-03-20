#if !NET35
using System;
using System.Reflection;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TestStack.BDDfy.Tests.Scanner
{
    [TestFixture]
    public class WhenStepScannerFactoryAsyncMethods
    {
        [Test]
        public void CallingAsyncTaskWhichThrowsIsObservedAndRethrown()
        {
            var stepAction = StepActionFactory.GetStepAction<SomeScenario>(o => AsyncTaskMethod(o));

            Assert.Throws<ArgumentException>(()=>stepAction(new SomeScenario()));
        }

        [Test]
        public void CallingAsyncVoidWhichThrowsIsObservedAndRethrown()
        {
            var stepAction = StepActionFactory.GetStepAction<SomeScenario>(s=>AsyncVoidMethod(s));

            Assert.Throws<ArgumentException>(() => stepAction(new SomeScenario()));
        }

        [Test]
        public void InvokingAsyncTaskWhichThrowsIsObservedAndRethrown()
        {
            var methodInfo = typeof(WhenStepScannerFactoryAsyncMethods).GetMethod("AsyncVoidMethod", BindingFlags.Instance | BindingFlags.NonPublic);
            var stepAction = StepActionFactory.GetStepAction(methodInfo, new object[] { new SomeScenario() });

            Assert.Throws<ArgumentException>(() => stepAction(this));
        }

        [Test]
        public void InvokingAsyncVoidWhichThrowsIsObservedAndRethrown()
        {
            var methodInfo = typeof(WhenStepScannerFactoryAsyncMethods).GetMethod("AsyncTaskMethod", BindingFlags.Instance | BindingFlags.NonPublic);
            var stepAction = StepActionFactory.GetStepAction(methodInfo, new object[] { new SomeScenario() });

            Assert.Throws<ArgumentException>(() => stepAction(this));
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
#endif