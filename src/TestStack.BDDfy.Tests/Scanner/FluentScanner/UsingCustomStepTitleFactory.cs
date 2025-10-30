using Shouldly;
using System.Linq;
using System.Reflection;
using TestStack.BDDfy.Abstractions;
using TestStack.BDDfy.Configuration;
using Xunit;

namespace TestStack.BDDfy.Tests.Scanner.FluentScanner
{
    [Collection("UseConfigurator")]
    public class UsingCustomStepTitleFactory
    {
        private class CustomStepTitleFactory : IStepTitleFactory
        {
            public StepTitle Create(
                string stepTextTemplate,
                bool includeInputsInStepTitle,
                MethodInfo methodInfo,
                StepArgument[] inputArguments,
                ITestContext testContext,
                string stepPrefix) => new StepTitle("Custom Step Title");

            public StepTitle Create(string title, string stepPrefix, ITestContext testContext) => new StepTitle(title);
        }

        [Fact]
        public void ShouldUseCustomStepTitleFactoryWhenSet()
        {
            Configurator.StepTitleFactory = new CustomStepTitleFactory();

            try
            {
                var story = new UsingCustomStepTitleFactory()
                .Given(_ => SomeState())
                .When(_ => SomethingHappens())
                .Then(_ => SomeResult())
                .BDDfy();

                story.Scenarios.Single().Steps.ElementAt(0).Title.ShouldBe("Custom Step Title");
                story.Scenarios.Single().Steps.ElementAt(1).Title.ShouldBe("Custom Step Title");
                story.Scenarios.Single().Steps.ElementAt(2).Title.ShouldBe("Custom Step Title");
            }
            finally
            {
                Configurator.StepTitleFactory = new DefaultStepTitleFactory();
            }

        }

        [Fact]
        public void ShouldUseCustomStepTitleFactoryWhenSetWithStepTitles()
        {
            Configurator.StepTitleFactory = new CustomStepTitleFactory();

            try
            {
                var story = new UsingCustomStepTitleFactory()
                .Given(_ => SomeState(), "Not this")
                .When(_ => SomethingHappens(), "Or this")
                .Then(_ => SomeResult(), "should not appear")
                .BDDfy();

                story.Scenarios.Single().Steps.ElementAt(0).Title.ShouldBe("Custom Step Title");
                story.Scenarios.Single().Steps.ElementAt(1).Title.ShouldBe("Custom Step Title");
                story.Scenarios.Single().Steps.ElementAt(2).Title.ShouldBe("Custom Step Title");
            }
            finally
            {
              Configurator.StepTitleFactory = new DefaultStepTitleFactory();

            }
        }

        [StepTitle("given from attribute")]
        private void SomeState()
        {
        }

        [StepTitle("when from attribute")]
        private void SomethingHappens()
        {
        }

        [StepTitle("then from attribute")]
        private void SomeResult()
        {
        }
    }
}