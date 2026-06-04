using Shouldly;
using Xunit;

namespace TestStack.BDDfy.Tests.Scanner;

public class WhenCombiningRunStepWithArgsAndExamples
{
    [Fact]
    public void ExampleValuesAndRunStepArgsBothGetPassedToMethod()
    {
        var story = new Scenario()
            .WithExamples(new ExampleTable("FirstExample") { 1, 2 })
            .BDDfy();

        var actualReport = story.Scenarios.Select(x => new
        {
            x.Title,
            x.Result,
            Steps = x.Steps.Select(x => x.Title).ToArray()
        }).ToArray();

        var expectedReport = new[]
        {
            new
            {
                Title = "Example values and run step args both get passed to method",
                Result = Result.Passed,
                Steps = new[] { 
                    "Given step with <first example> passed as parameter value 1", 
                    "And step with <first example> passed as parameter value 2", 
                    "Then example value is set", 
                    "And run step arg is set" }
            },
            new
            {
                Title = "Example values and run step args both get passed to method",
                Result = Result.Passed,
                Steps = new[] { 
                    "Given step with <first example> passed as parameter value 1", 
                    "And step with <first example> passed as parameter value 2", 
                    "Then example value is set", 
                    "And run step arg is set" }
            }
        };

        actualReport.ShouldBeEquivalentTo(expectedReport);
    }

    private class Scenario
    {
        private int _firstExample;
        private string? _input;

        [RunStepWithArgs("value 1")]
        [RunStepWithArgs("value 2")]
        public void GivenStepWith__FirstExample__PassedAsParameter(int firstExample, string input)
        {
            _firstExample = firstExample;
            _input = input;
        }

        public void ThenExampleValueIsSet()
        {
            _firstExample.ShouldBeGreaterThan(0);
        }

        public void AndRunStepArgIsSet()
        {
            _input.ShouldBe("value 2");
        }
    }
}
