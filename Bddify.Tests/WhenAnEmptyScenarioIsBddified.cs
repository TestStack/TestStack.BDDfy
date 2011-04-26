using NUnit.Framework;
using Bddify.Core;

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
            var scenario = new ScenarioWithNoSteps();
            scenario.Bddify();
        }
    }
}