using NUnit.Framework;
using TestStack.BDDfy.Configuration;

namespace TestStack.BDDfy.Tests
{
    [SetUpFixture]
    public class Setup
    {
        [SetUp]
        public void RunBeforeAnyTests()
        {
            // somehow the scenario id keeps increasing on TC
            // resetting here explicitly
            Configurator.IdGenerator.Reset();
        }
    }
}