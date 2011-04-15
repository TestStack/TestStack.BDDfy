using Bddify.Core;

namespace Bddify.Tests.Story
{
    [WithStory(typeof(StoryDouble))]
    [RunScenarioWithArgs(1, 2)]
    [RunScenarioWithArgs(3, 6)]
    public class ScenarioInStoryWithArgs
    {
        private int _result;
        private int _multiplicant;

        public void RunScenarioWithArgs(int multiplicant, int result)
        {
            _multiplicant = multiplicant;
            _result = result;
        }

        void WhenScenarioIsToBeRunWithMultipleArgumentSets()
        {
            
        }

        void ThenBddifyAddsOneScenarioPerArgumentSetForTheStory()
        {
            
        }
    }
}