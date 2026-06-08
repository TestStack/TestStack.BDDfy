using Shouldly;
using Xunit;

namespace TestStack.BDDfy.Tests.Scanner.Examples
{
    /// <summary>
    /// Verifies that fluent steps using closure-captured method parameters
    /// from [InlineData] / [TestCase] retain correct values at execution time.
    /// Regression test: ParameterizedTestDetector must not overwrite closure values
    /// when the parameter is not stored as a field on the test object.
    /// </summary>
    public class FluentClosureParameterTests
    {
        private string? _capturedRoute;
        private int _capturedStatus;

        [Theory]
        [InlineData("/api/orders", 200)]
        [InlineData("/api/missing", 404)]
        public void Multi_parameter_values_should_not_be_corrupted(string endpoint, int statusCode)
        {
            this.Given(_ => A_request_is_made_to(endpoint))
                .When(_ => The_response_status_is(statusCode))
                .Then(_ => The_resolved_route_should_be(endpoint))
                .And(_ => The_status_should_be(statusCode))
                .BDDfy();
        }

        private void A_request_is_made_to(string route)
        {
            _capturedRoute = route;
        }

        private void The_response_status_is(int statusCode)
        {
            _capturedStatus = statusCode;
        }

        private void The_resolved_route_should_be(params string[] endpoint)
        {
            new[] { _capturedRoute }.ShouldBe(endpoint);
        }

        private void The_status_should_be(int expected)
        {
            _capturedStatus.ShouldBe(expected);
        }
    }
}
