using System.Threading.Tasks;
using TestStack.BDDfy.Configuration;
using Xunit;

namespace TestStack.BDDfy.Samples
{
    public class CanRunAsyncVoidSteps
    {
        [Fact]
        internal void Run() => this.BDDfy();
        internal void SetUp() => Configurator.AsyncVoidSupportEnabled = false;
        internal void TearDown() => Configurator.AsyncVoidSupportEnabled = true;

        internal async void GivenNonAsyncStep() => await Task.CompletedTask;
        internal async void WhenSomethingHappens() => await Task.CompletedTask;
        internal async void ThenAssertSomething()  => await Task.CompletedTask;
    }
}