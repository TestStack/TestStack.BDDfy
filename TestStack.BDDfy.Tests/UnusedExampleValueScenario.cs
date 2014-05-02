namespace TestStack.BDDfy.Tests
{
    using NUnit.Framework;

    using Shouldly;

    using TestStack.BDDfy.Processors;

    [TestFixture]
    public class UnusedExampleValueScenario
    {
        [Test]
        public void WhenExampleIsNotUsedItThrows()
        {
            var ex = Should.Throw<UnusedExampleException>(() => this.Given("Foo").WithExamples(new ExampleTable("Example 1") { 1 }).BDDfy());

            ex.Message.ShouldBe("Example Column 'Example 1' is unused, all examples should be consumed by the test (have you misspelt a field or property?)");
        }
    }
}