using Shouldly;
using Xunit;

namespace TestStack.BDDfy.Tests.Scanner.FluentScanner
{
    /// <summary>
    /// Regression test for https://github.com/TestStack/TestStack.BDDfy/issues/257
    /// When a step method has a parameter whose name matches an example column name
    /// (case-insensitive), the framework should not throw InvalidOperationException
    /// ("Sequence contains more than one matching element") during title generation.
    /// </summary>
    public class ExampleColumnNameMatchingMethodParameterName
    {
        private readonly string _existing = "";
        private string _resource = "";

        private void GivenThereIsOnly__existing__ResourceInDatabase(string resource)
        {
            _resource = resource;
        }

        private void WhenSomethingHappens() { }

        private void ThenItShouldWork()
        {
            _existing.ShouldNotBeNullOrEmpty();
            _resource.ShouldNotBeNullOrEmpty();
        }

        [Fact]
        public void ShouldNotThrowWhenParameterNameMatchesExampleColumn()
        {
            this.Given(_ => _.GivenThereIsOnly__existing__ResourceInDatabase(_resource))
                .When(_ => _.WhenSomethingHappens())
                .Then(_ => _.ThenItShouldWork())
                .WithExamples(new ExampleTable("Resource", "Existing")
                {
                    { "MyResource", "item1" }
                })
                .BDDfy();
        }
    }
}
