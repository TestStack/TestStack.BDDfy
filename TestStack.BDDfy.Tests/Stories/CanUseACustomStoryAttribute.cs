using NUnit.Framework;
using Shouldly;
using Xunit;

namespace TestStack.BDDfy.Tests.Stories
{
    class InOrderToStoryAttribute : StoryNarrativeAttribute
    {
        // ReSharper disable InconsistentNaming
        private const string As_a_prefix = "As a";
        private const string In_order_to_prefix = "In order to";
        private const string I_want_prefix = "I want";
        // ReSharper restore InconsistentNaming

        public string InOrderTo
        {
            get { return Narrative1; }
            set { Narrative1 = CleanseProperty(value, In_order_to_prefix); }
        }

        public string AsA
        {
            get { return Narrative2; }
            set { Narrative2 = CleanseProperty(value, As_a_prefix); }
        }

        public string IWant
        {
            get { return Narrative3; }
            set { Narrative3 = CleanseProperty(value, I_want_prefix); }
        }
    }

    [InOrderToStory(
        InOrderTo = "do something",
        AsA = "programmer",
        IWant = "this to work")]
    public class CanUseACustomStoryAttribute
    {
        [Fact]
        public void When_InOrderTo_is_specified_the_InOrderTo_syntax_is_used()
        {
            var story = new DummyScenario().BDDfy<CanUseACustomStoryAttribute>();

            story.Metadata.Narrative1.ShouldBe("In order to do something");
            story.Metadata.Narrative2.ShouldBe("As a programmer");
            story.Metadata.Narrative3.ShouldBe("I want this to work");
        }
    }
}
