using System;
using System.Runtime.CompilerServices;
using ApprovalTests;
using Shouldly;
using TestStack.BDDfy.Reporters;
using Xunit;

namespace TestStack.BDDfy.Tests.Scanner.Examples
{
    public class FluentWithExamplesAtEnd
    {
        [Fact]
        public void FluentCanBeUsedWithExamples()
        {
            var story = this
                .Given(_ => MethodTaking__ExampleIntA__(1), false)
                .When(_ => WhenEmptyMethod())
                .Then(_ => ThenAllIsGood())
                .BDDfy();

            var textReporter = new TextReporter();
            textReporter.Process(story);
            Approvals.Verify(textReporter.ToString());
        }

        // This test crashes BDDfy or causes an infinite loop
        [Fact]
        public void FluentCanBeUsedWithExamplesEndingInANumber() {
            var story = this
                .Given(_ => MethodTaking__ExampleInt1__(1), false)
                .When(_ => WhenEmptyMethod())
                .Then(_ => ThenAllIsGood())
                .BDDfy();

            var textReporter = new TextReporter();
            textReporter.Process(story);
            Approvals.Verify(textReporter.ToString());
        }

        private void ThenAllIsGood()
        {
        }

        private void WhenEmptyMethod()
        {
        }

        // Ending an example name with a number seems to cause problems in BDDfy
        private void MethodTaking__ExampleInt1__(int exampleInt)
        {
            exampleInt.ShouldBeInRange(1, 2);
        }

        private void MethodTaking__ExampleIntA__(int exampleInt) {
            exampleInt.ShouldBeInRange(1, 2);
        }
    }
}