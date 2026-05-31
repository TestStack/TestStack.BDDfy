using System.Linq;
using Shouldly;
using Xunit;

namespace TestStack.BDDfy.Tests
{
    public class WithTagsExtensionsTests
    {
        [Fact]
        public void WithTags_AddsTags_ToTestContext()
        {
            var testObject = new TestClass();
            var result = testObject.WithTags("tag1", "tag2");

            result.ShouldBeSameAs(testObject);
            var context = TestContext.GetContext(testObject);
            context.Tags.ShouldContain("tag1");
            context.Tags.ShouldContain("tag2");
        }

        [Fact]
        public void WithTags_CanBeCalledMultipleTimes()
        {
            var testObject = new TestClass();
            testObject.WithTags("a").WithTags("b", "c");

            var context = TestContext.GetContext(testObject);
            context.Tags.Count.ShouldBe(3);
        }

        private class TestClass { }
    }
}
