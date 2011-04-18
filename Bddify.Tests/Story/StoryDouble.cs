using Bddify.Core;

namespace Bddify.Tests.Story
{
    [Story(
        AsA = "As a good programmer",
        IWant = "I want to be able to write my stories as part of my BDD tests",
        SoThat = "So I can get business readable requirements")]
    [WithScenario(typeof(FirstScenario))]
    [WithScenario(typeof(ScenarioInStoryWithArgs))]
    public class StoryDouble
    {
    }
}