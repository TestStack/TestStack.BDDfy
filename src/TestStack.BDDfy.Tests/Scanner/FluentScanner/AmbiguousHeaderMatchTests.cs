using System.Reflection;
using Shouldly;
using Xunit;

namespace TestStack.BDDfy.Tests.Scanner.FluentScanner
{
    public class AmbiguousHeaderMatchTests
    {
        private readonly int _count = 0;

        [Fact]
        public void ThrowsWhenMultipleHeadersMatchParameterName()
        {
            // Act & Assert
            var exception = Should.Throw<AmbiguousMatchException>(() =>
            {
                this.Given(_ => GivenInput(_count))  // Will try to bind _count to both "count" and "Count" headers
                    .WithExamples(new ExampleTable("count", "Count") // Deliberately ambiguous headers
                    {
                        { 5, 10 }
                    })
                    .BDDfy();
            });

            exception.Message.ShouldBe("Duplicate headers detected. count, Count");
        }

        private void GivenInput(int count)
        {
            // The method exists just to trigger the ambiguous header match
        }
    }
}