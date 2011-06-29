using NUnit.Framework;

namespace Bddify.Tests
{
    [TestFixture]
    public class WhenAnEmptyScenarioIsBddified
    {
        private class ScenarioWithNoSteps
        {
        }

        [Test]
        public void ThenNoExeptionIsThrown()
        {
            new ScenarioWithNoSteps().Bddify();
        }
    }
}