using Shouldly;
using TestStack.BDDfy.Reporters;
using Xunit;

namespace TestStack.BDDfy.Tests.Scanner.Examples
{
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
        public void GivenStepWith__FirstExample__PassedAsParameter(int firstExample)
        {
            firstExample.ShouldBeOneOf(1, 2);
        }

        public void AndGivenStepWith__SecondExample__AccessedViaProperty()
        {
            SecondExample.ShouldBeOneOf("foo", "bar");
        }

        [Fact]
        public void Run()
        {
            var reporter = new TextReporter();
            reporter.Process(_story);
            reporter.ToString().ShouldMatchApproved();
        }
    }
}