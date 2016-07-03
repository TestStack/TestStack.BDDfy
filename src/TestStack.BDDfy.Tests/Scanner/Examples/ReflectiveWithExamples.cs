#if Approvals
using Shouldly;
using TestStack.BDDfy.Reporters;
using Xunit;

namespace TestStack.BDDfy.Tests.Scanner.Examples
{
    public class ReflectiveWithExamples
    {
        public string SecondExample { get; set; }

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
            var story = this
                .WithExamples(new ExampleTable("First Example", "Second Example")
                {
                    {1, "foo"},
                    {2, "bar"}
                })
                .BDDfy();

            var reporter = new TextReporter();
            reporter.Process(story);
            reporter.ToString().ShouldMatchApproved();
        }
    }
}
#endif