using NUnit.Framework;

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

    [TestFixture]
    [InOrderToStory(
        InOrderTo = "do something",
        AsA = "programmer",
        IWant = "this to work")]
    public class CanUseACustomStoryAttribute
    {
        [Test]
        public void When_InOrderTo_is_specified_the_InOrderTo_syntax_is_used()
        {
            var story = new DummyScenario().BDDfy<CanUseACustomStoryAttribute>();

            Assert.That(story.Metadata.Narrative1, Is.EqualTo("In order to do something"));
            Assert.That(story.Metadata.Narrative2, Is.EqualTo("As a programmer"));
            Assert.That(story.Metadata.Narrative3, Is.EqualTo("I want this to work"));
        }
    }
}
