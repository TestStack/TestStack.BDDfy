using Xunit;

namespace TestStack.BDDfy.Tests
{
    public class WhenAnEmptyScenarioIsBddified
    {
        private class ScenarioWithNoSteps
        {
        }

        [Fact]
        public void ThenNoExeptionIsThrown()
        {
            new ScenarioWithNoSteps().BDDfy();
        }
    }
}