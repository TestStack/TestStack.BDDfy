using System;
using System.Linq;
using System.Threading.Tasks;
using TestStack.BDDfy.Tests.Stories;
using Xunit;

namespace TestStack.BDDfy.Tests.Concurrency
{
    public class ParallelRunnerScenario
    {
        [Fact]
        public async Task CanHandleMultipleThreadsExecutingBddfyConcurrently()
        {
            try
            {
                await Task.WhenAll(
                    Enumerable.Range(0, 100)
                        .Select(_ => Task.Run(() => new DummyScenario().BDDfy<ParallelRunnerScenario>())));
            }
            catch (Exception e)
            {
                Assert.False(true, "Threw exception " + e);
            }
        }
    }
}
