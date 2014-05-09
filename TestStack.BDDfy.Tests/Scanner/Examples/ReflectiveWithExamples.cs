using ApprovalTests;
using NUnit.Framework;
using Shouldly;
using TestStack.BDDfy.Reporters;

namespace TestStack.BDDfy.Tests.Scanner.Examples
{
    [TestFixture]
    public class ReflectiveWithExamples
    {
        public string SecondExample { get; set; }

        public void GivenStepWith__FirstExample__PassedAsParameter(int firstExample)
        {
            firstExample.ShouldBeOneOf(1, 2);
        }

        public void GivenStepWith__SecondExample__AccessedViaProperty()
        {
            SecondExample.ShouldBeOneOf("foo", "bar");
        }

        [Test]
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
            Approvals.Verify(reporter.ToString());
        }
    }
}