using ApprovalTests;
using NUnit.Framework;
using TestStack.BDDfy.Reporters;

namespace TestStack.BDDfy.Tests
{
    [TestFixture]
    public class TagsTests
    {
        [Test]
        public void TagsAreReportedInTextReport()
        {
            var story = this.Given(_ => GivenAStep())
                .WithTags("Tag1", "Tag 2")
                .BDDfy();

            var textReporter = new TextReporter();
            textReporter.Process(story);

            Approvals.Verify(textReporter.ToString());
        }

        private void GivenAStep()
        {
            
        }
    }
}