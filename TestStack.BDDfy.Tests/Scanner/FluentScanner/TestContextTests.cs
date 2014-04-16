using NUnit.Framework;
using Shouldly;

namespace TestStack.BDDfy.Tests.Scanner.FluentScanner
{
    [TestFixture]
    public class TestContextTests
    {
        [Test]
        public void ShouldUnwrapTestObjectIfAlreadyIsATestObject()
        {
            var exampleTable = new ExampleTable();

            var fluentScanner = (ITestContext)this
                .WithExamples(exampleTable)
                .Given("Test");

            fluentScanner.Examples.ShouldBeSameAs(exampleTable);
            fluentScanner.TestObject.ShouldBeSameAs(this);
        }
    }
}