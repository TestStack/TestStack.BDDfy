using NUnit.Framework;

namespace Bddify.Tests
{
    public class WhenAnEmptyScenarioIsBddified
    {
        private class ScenarioWithNoSteps
        {
        }

        [Test]
        public void ThenNoExeptionIsThrown()
        {
            typeof(ScenarioWithNoSteps).Bddify();
        }
    }
}