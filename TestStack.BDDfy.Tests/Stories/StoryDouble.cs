using System.Linq;
using Shouldly;
using Xunit;

namespace TestStack.BDDfy.Tests.Stories
{
    [Story(
        AsA = "As a good programmer",
        IWant = "I want to be able to write my stories as part of my BDD tests",
        SoThat = "So I can get business readable requirements")]
    public class StoryDouble
    {
        [Fact]
        public void ScanningAScenarioWithoutArgsFromAStoryClass()
        {
            var testObject = new DummyScenario();
            var scanner = new DefaultScanner(TestContext.GetContext(testObject), new ReflectiveScenarioScanner(new DefaultMethodNameStepScanner()));
            var story = scanner.Scan();

            story.Metadata.ShouldBeAssignableTo<StoryDouble>();
            story.Scenarios.Count().ShouldBe(1);
            story.Scenarios.Single().TestObject.ShouldBeAssignableTo<DummyScenario>();
        }
    }
}