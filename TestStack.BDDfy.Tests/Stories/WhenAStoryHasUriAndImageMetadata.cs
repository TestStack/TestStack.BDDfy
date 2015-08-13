using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Shouldly;

namespace TestStack.BDDfy.Tests.Stories
{
    [Story(
        AsA = "programmer",
        IWant = "to attach an uri and image metadata to a story",
        SoThat = "my output report communicates better to my stakeholders",
        StoryUri = "http://teststoryuri.com.au",
        ImageUri = "http://teststoryuri.com.au/storyimg.png")]
    public class WhenAStoryHasUriAndImageMetadata
    {
        [Fact]
        public void Then_it_is_injected_by_BDDfy()
        {
            var story = new DummyScenario().BDDfy<WhenAStoryHasUriAndImageMetadata>();
            story.Metadata.StoryUri.ShouldBe("http://teststoryuri.com.au");
            story.Metadata.ImageUri.ShouldBe("http://teststoryuri.com.au/storyimg.png");
        }
    }
}
