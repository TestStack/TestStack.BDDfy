using Shouldly;
using System.Diagnostics.CodeAnalysis;
using TestStack.BDDfy.Reporters;
using Xunit;

namespace TestStack.BDDfy.Tests.Scanner.Examples
{
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public class ReflectiveWithExamples
    {
        private readonly Story _story;

        public string SecondExample { get; set; }
        public ReflectiveWithExamples()
        {
            _story = this
                .WithExamples(new ExampleTable("First Example", "Second Example")
                {
                    {1, "foo"},
                    {2, "bar"}
                })
                .BDDfy();
        }

        internal void GivenStepWith__FirstExample__PassedAsParameter(int firstExample) => firstExample.ShouldBeOneOf(1, 2);

        internal void AndGivenStepWith__SecondExample__AccessedViaProperty() => SecondExample.ShouldBeOneOf("foo", "bar");

        [Fact]
        public void RunExamplesUsingReflectiveScanner()
        {
            var reporter = new TextReporter();
            reporter.Process(_story);
            reporter.ToString().ShouldMatchApproved();
        }
    }
}