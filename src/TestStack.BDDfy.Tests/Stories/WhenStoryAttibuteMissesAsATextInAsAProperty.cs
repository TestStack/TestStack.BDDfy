using Shouldly;
using Xunit;

namespace TestStack.BDDfy.Tests.Stories
{
    [Story(
        AsA = "programmer",
        IWant = "I want the missing 'As a' to be added to story metadata",
        SoThat = "So that I don't have to duplicate it on the string")]
    public class WhenStoryAttibuteMissesAsATextInAsAProperty
    {
        [Fact]
        public void Then_it_is_injected_by_BDDfy()
        {
            var story = new DummyScenario().BDDfy<WhenStoryAttibuteMissesAsATextInAsAProperty>();

            story.Metadata.Narrative1.ShouldBe("As a programmer");
            story.Metadata.Narrative2.ShouldBe("I want the missing 'As a' to be added to story metadata");
            story.Metadata.Narrative3.ShouldBe("So that I don't have to duplicate it on the string");
        }
    }
}