using NUnit.Framework;

namespace BDDfy.Tests
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
            new ScenarioWithNoSteps().BDDfy();
        }
    }
}