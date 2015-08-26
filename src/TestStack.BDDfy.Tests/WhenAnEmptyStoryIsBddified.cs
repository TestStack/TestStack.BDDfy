using Xunit;

namespace TestStack.BDDfy.Tests
{
    [Story(
        AsA = "As a programmer",
        IWant = "I want to first create an empty story",
        SoThat = "So that I can do test first development")]
    public class WhenAnEmptyStoryIsBddified
    {
        [Fact]
        public void NoExeptionIsThrown()
        {
            this.BDDfy();
        }
    }
}