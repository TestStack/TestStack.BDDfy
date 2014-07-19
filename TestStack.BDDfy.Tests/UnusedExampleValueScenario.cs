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
            var ex = Should.Throw<UnusedExampleException>(() => 
                this.Given("Foo")
                .WithExamples(new ExampleTable("Example 1") { 1 }).BDDfy());

            ex.Message.ShouldBe("Example Column 'Example 1' is unused, all examples should be consumed by the test (have you misspelt a field or property?)");
        }

        [Test]
        public void NullableEnumIsUsedProperly()
        {
            SomeEnum? nullableEnum = null;
            string anotherExample = null;

            this.Given(_ => GivenANullableEnumExample(nullableEnum))
                .Then(_ => AnotherValueIsChanged(anotherExample))
                .WithExamples(new ExampleTable("Nullable Enum", "Another Example")
                {
                    {SomeEnum.Value, string.Empty},
                    {null, string.Empty}
                })
                .BDDfy();
        }

        private void AnotherValueIsChanged(string anotherExample)
        {
        }

        private void GivenANullableEnumExample(SomeEnum? nullableEnum)
        {
        }

        public enum SomeEnum
        {
            Value
        }
    }
}