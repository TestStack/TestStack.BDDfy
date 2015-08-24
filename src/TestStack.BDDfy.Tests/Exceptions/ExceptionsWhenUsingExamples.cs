using System;
using Shouldly;
using Xunit;

namespace TestStack.BDDfy.Tests.Exceptions
{
    public class ExceptionsWhenUsingExamples
    {
        [Fact]
        public void CorrectlyReportsErrorWhenAllStepsNotRun()
        {
            Should.Throw<InvalidOperationException>(() =>
            {
                var exampleTable = new ExampleTable("Arg1", "Arg2")
                {
                    {"foo", "bar"},
                    {"foo", "bar"}
                };

                string arg1 = null;
                string arg2 = null;
                var arg3 = default(string);
                this.Given(_ => AFailingStep(arg1), "Given a failing step")
                    .Then(_ => NonExecutedStep(arg2), "Then multiple assertions")
                    .And(_ => NonExecutedStep(arg3), "The second one blows up")
                    .WithExamples(exampleTable)
                    .BDDfy();
            });
        }

        private void NonExecutedStep(string s)
        {
            
        }

        private void AFailingStep(string s)
        {
            throw new InvalidOperationException();
        }
    }
}