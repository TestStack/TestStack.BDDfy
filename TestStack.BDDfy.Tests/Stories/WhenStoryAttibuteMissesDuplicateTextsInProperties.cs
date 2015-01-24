using Shouldly;
using Xunit;

namespace TestStack.BDDfy.Tests.Stories
{
    [Story(
        AsA = "programmer",
        IWant = "the missing texts to be added to story metadata",
        SoThat = "I don't have to duplicate it on the string")]
    public class WhenStoryAttibuteMissesDuplicateTextsInProperties
    {
        [Fact]
        public void Then_it_is_injected_by_BDDfy()
        {
            var story = new DummyScenario().BDDfy<WhenStoryAttibuteMissesDuplicateTextsInProperties>();

            story.Metadata.Narrative1.ShouldBe("As a programmer");
            story.Metadata.Narrative2.ShouldBe("I want the missing texts to be added to story metadata");
            story.Metadata.Narrative3.ShouldBe("So that I don't have to duplicate it on the string");
        }
    }
}