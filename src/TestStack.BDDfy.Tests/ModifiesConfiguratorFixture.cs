using TestStack.BDDfy.Tests.Concurrency;
using Xunit;

namespace TestStack.BDDfy.Tests
{
    [CollectionDefinition(TestCollectionName.ModifiesConfigurator, DisableParallelization = true)]
    public class ModifiesConfiguratorFixture
    {
    }
}