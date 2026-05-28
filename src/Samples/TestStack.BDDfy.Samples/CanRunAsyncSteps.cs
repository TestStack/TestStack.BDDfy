using System;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace TestStack.BDDfy.Samples
{
    public class CanRunAsyncSteps
    {
        private object _sut;

        internal async void GivenSomeAsyncSetup()
        {
            _sut = await CreateSut();
        }

        internal void ThenBddfyHasWaitedForThatSetupToCompleteBeforeContinuing()
        {
            _sut.ShouldNotBe(null);
        }

        internal async void AndThenBddfyShouldCaptureExceptionsThrownInAsyncMethod()
        {
            await Task.Yield();
            throw new Exception("Exception in async void method!!");
        }

        private static async Task<object> CreateSut()
        {
            await Task.Delay(500);
            return new object();
        }

        [Fact]
        public void Run()
        {
            var engine = this.LazyBDDfy();
            var exception = Should.Throw<Exception>(engine.Run);

            exception.Message.ShouldBe("Exception in async void method!!");
        }
    }
}