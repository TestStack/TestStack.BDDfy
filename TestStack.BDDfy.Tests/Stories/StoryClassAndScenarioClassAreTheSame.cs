using System.Linq;
using Shouldly;
using Xunit;

namespace TestStack.BDDfy.Tests.Stories
{
    public class StoryClassAndScenarioClassAreTheSame
    {
        private Story _story;

        [Story(
            AsA = "As a story with no scenarios specified using attributes",
            IWant = "I want to be considered as a scenario",
            SoThat = "So that single scenario stories do not have to separate into two classes")]
        public class StoryAsScenario
        {
            [Then(StepTitle = "See?! I am a normal scenario even though I have been decorated with StoryAttribute")]
            void SeeIAmAScenario()
            {
            }
        }

        void WhenTheStoryIsBddified()
        {
            _story = new StoryAsScenario().BDDfy();
        }

        void ThenStoryIsReturnedAsAStory()
        {
            _story.Metadata.Type.ShouldBe(typeof(StoryAsScenario));
        }

        [AndThen(StepTitle = "and as a scenario")]
        void andAsAScenario()
        {
            _story.Scenarios.Count().ShouldBe(1);
            var scenario = _story.Scenarios.First();
            scenario.TestObject.ShouldBeAssignableTo<StoryAsScenario>();
        }

        void andTheNarrativeIsReturnedAsExpected()
        {
            var expectedNarrative = (StoryAttribute)typeof(StoryAsScenario).GetCustomAttributes(typeof(StoryAttribute), false).First();
            _story.Metadata.ShouldNotBe(null);
            _story.Metadata.Narrative1.ShouldBe(expectedNarrative.AsA);
            _story.Metadata.Narrative2.ShouldBe(expectedNarrative.IWant);
            _story.Metadata.Narrative3.ShouldBe(expectedNarrative.SoThat);
        }

        [Fact]
        public void Execute()
        {
            try
            {
                // we need TestObject for this test; so I disable StoryCache processor for this one test
                BDDfy.Configuration.Configurator.Processors.StoryCache.Disable();

                this.BDDfy();
            }
            finally
            {
                BDDfy.Configuration.Configurator.Processors.StoryCache.Enable();
            }
        }
    }
}