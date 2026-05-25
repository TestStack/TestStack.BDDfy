using System.Linq;
using Shouldly;
using TestStack.BDDfy.Configuration;
using TestStack.BDDfy.Tests.Concurrency;
using Xunit;

namespace TestStack.BDDfy.Tests.Scanner.FluentScanner
{
    [Collection(TestCollectionName.ModifiesConfigurator)]
    public class StepTitleTests
    {
        private string _state;

        [Fact]
        public void UseConfiguration_IncludeInputsInStepTitle()
        {
            try
            {
                Configurator.StepTitleFactory.IncludeInputsInStepTitle = false;
                FooClass something = new();
                var story = this
                    .Given(_=>something.Sub.GivenWithStepTitleAndArgument(1))
                    .When(_ => something.Sub.ActionWithArgument("foo"))
                    .And(_ => something.Sub.ActionWithArgumentsDisabledInTitle("foo"))
                    .And(_ => something.Sub.ActionWithTemplateTitleAndArguments("foo"))
                    .And(_ => something.Sub.ActionWithArgumentsEnabledInTitle("foo"))
                    .BDDfy();

                var actualTitles = story.Scenarios.Single().Steps.Select(s => s.Title).ToArray();
                var expectedTitles = new[]
                {
                    "Given step title with 1 args",
                    "When with arg",
                    "And with arg",
                    "And with foo arg",
                    "And with arg foo"
                };

                actualTitles.ShouldBeEquivalentTo(expectedTitles); ;
            }
            catch
            {
                throw;
            }
            finally
            {
                Configurator.StepTitleFactory.IncludeInputsInStepTitle = true;
            }

        }

        [Fact]
        public void MethodCallInStepTitle()
        {
            FooClass something = new();
            var story = this
                .Given(_ => GivenWeMutateSomeState())
                .When(_ => something.Sub.SomethingHappens())
                .And(_ => something.Sub.SomethingWithDifferentTitle())
                .Then(_ => ThenTitleHas(AMethodCall()))
                .And(_ => something.Sub.ActionWithArgument("foo"))
                .And(_ => something.Sub.ActionWithArgumentsDisabledInTitle("foo"))
                .And(_ => something.Sub.ActionWithTemplateTitleAndArguments("foo"))
                .BDDfy();

            var actualTitles = story.Scenarios.Single().Steps.Select(s => s.Title).ToArray();
            var expectedTitles = new[]
            {
                "Given we mutate some state",
                "When something happens",
                "And different title",
                "Then title has Mutated state",
                "And with arg foo",
                "And with arg",
                "And with foo arg"
            };

            actualTitles.ShouldBeEquivalentTo(expectedTitles);
        }

        public class FooClass
        {
            public FooClass()
            {
                Sub = new BarClass();
            }

            public BarClass Sub { get; set; }
        }

        public class BarClass
        {
            public void SomethingHappens()
            {
                
            }

            [StepTitle("Different title")]
            public void SomethingWithDifferentTitle()
            {
            }

            [StepTitle("With arg")]
            public void ActionWithArgument(string arg)
            {
            }

            [StepTitle("With arg", false)]
            public void ActionWithArgumentsDisabledInTitle(string arg)
            {
            }

            [StepTitle("With arg", true)]
            public void ActionWithArgumentsEnabledInTitle(string arg)
            {
            }

            [StepTitle("With {0} arg", false)]
            public void ActionWithTemplateTitleAndArguments(string arg)
            {
            }

            [Given("step title with {0} args")]
            public void GivenWithStepTitleAndArgument(int count)
            { }
        }

        private string AMethodCall()
        {
            return _state;
        }

        private void GivenWeMutateSomeState()
        {
            _state = "Mutated state";
        }

        private void ThenTitleHas(string result)
        {
            result.ShouldBe(_state);
        }
    }
}