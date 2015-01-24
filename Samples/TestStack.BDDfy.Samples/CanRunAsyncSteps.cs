using System;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace TestStack.BDDfy.Samples
{
    public class CanRunAsyncSteps
    {
        private Sut _sut;

        public async void GivenSomeAsyncSetup()
        {
            _sut = await CreateSut();
        }

        public void ThenBddfyHasWaitedForThatSetupToCompleteBeforeContinuing()
        {
            _sut.ShouldNotBe(null);
        }

        public async Task AndThenBddfyShouldCaptureExceptionsThrownInAsyncMethod()
        {
            await Task.Yield();
            throw new Exception("Exception in async void method!!");
        }

        private async Task<Sut> CreateSut()
        {
            await Task.Delay(500);
            return new Sut();
        }

        [Fact]
        public void Run()
        {
            var engine = this.LazyBDDfy();
            var exception = Should.Throw<Exception>(() => engine.Run());

            exception.Message.ShouldBe("Exception in async void method!!");
        }

        internal class Sut
        {
        }
    }
}