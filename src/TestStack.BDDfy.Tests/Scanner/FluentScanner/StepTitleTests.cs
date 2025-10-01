using System;
using System.Linq;

using Shouldly;
using TestStack.BDDfy.Configuration;
using Xunit;

namespace TestStack.BDDfy.Tests.Scanner.FluentScanner
{
    public class StepTitleTests:IDisposable
    {
        public StepTitleTests()
        {
            Configurator.Scanners.SetDefaultStepTitleCreatorFunction(null);
        }
        private string _mutatedState;

        [Fact]
        public void MethodCallInStepTitle()
        {
            FooClass something = new FooClass();
            var story = this
                .Given(_ => GivenWeMutateSomeState())
                .When(_ => something.Sub.SomethingHappens())
                .And(_ => something.Sub.SomethingWithDifferentTitle())
                .Then(_ => ThenTitleHas(AMethodCall()))
                .And(_ => something.Sub.SomethingWithArg("foo"))
                .And(_ => something.Sub.SomethingWithArg2("foo"))
                .And(_ => something.Sub.SomethingWithArg3("foo"))
                .BDDfy();

            story.Scenarios.Single().Steps.ElementAt(2).Title.ShouldBe("And different title");
            story.Scenarios.Single().Steps.ElementAt(3).Title.ShouldBe("Then title has Mutated state");
            story.Scenarios.Single().Steps.ElementAt(4).Title.ShouldBe("And with arg foo");
            story.Scenarios.Single().Steps.ElementAt(5).Title.ShouldBe("And with arg");
            story.Scenarios.Single().Steps.ElementAt(6).Title.ShouldBe("And with foo arg");
        }

        [Fact]
        public void TitleFunctionCanBeOverriden()
        {
            FooClass something = new FooClass();
            var context = TestContext.GetContext(something);
            
            Configurator.Scanners.SetDefaultStepTitleCreatorFunction((a, b, c, d, e) => new StepTitle("hello"));
            new FluentStepBuilder<FooClass>(something);
            var story = something
          .Given(_ => GivenWeMutateSomeState())
           .When(_ => something.Sub.SomethingHappens())
           .And(_ => something.Sub.SomethingWithDifferentTitle())
           .Then(_ => ThenTitleHas(AMethodCall()))
           .And(_ => something.Sub.SomethingWithArg("foo"))
           .And(_ => something.Sub.SomethingWithArg2("foo"))
           .And(_ => something.Sub.SomethingWithArg3("foo"))
           .BDDfy();

            story.Scenarios.Single().Steps.ElementAt(2).Title.ShouldBe("hello");
            story.Scenarios.Single().Steps.ElementAt(3).Title.ShouldBe("hello");
            story.Scenarios.Single().Steps.ElementAt(4).Title.ShouldBe("hello");
            story.Scenarios.Single().Steps.ElementAt(5).Title.ShouldBe("hello");
            story.Scenarios.Single().Steps.ElementAt(6).Title.ShouldBe("hello");
        }

        [Fact]
        public void TitleFunctionCanBeOverridenAndUseParameters()
        {
            GivenWeMutateSomeState();

            FooClass something = new FooClass();
            var context = TestContext.GetContext(something);
          
            Configurator.Scanners.SetDefaultStepTitleCreatorFunction((a, b, c, d, e) => new StepTitle(e + " " + c.Name + " " + string.Join(",", d.Select(arg => arg.Value).ToArray())));
            new FluentStepBuilder<FooClass>(something);
            var story = something
               .Given(_ => GivenWeMutateSomeState())
                .When(_ => something.Sub.SomethingHappens())
                .And(_ => something.Sub.SomethingWithDifferentTitle())
                .Then(_ => ThenTitleHas(AMethodCall()))
                .And(_ => something.Sub.SomethingWithArg("foo"))
                .And(_ => something.Sub.SomethingWithArg2("foo2"))
                .And(_ => something.Sub.SomethingWithArg3("foo3"))
                .BDDfy();

            story.Scenarios.Single().Steps.ElementAt(0).Title.ShouldBe("Given GivenWeMutateSomeState ");
            story.Scenarios.Single().Steps.ElementAt(1).Title.ShouldBe("When SomethingHappens ");
            story.Scenarios.Single().Steps.ElementAt(2).Title.ShouldBe("And SomethingWithDifferentTitle ");
            story.Scenarios.Single().Steps.ElementAt(3).Title.ShouldBe("Then ThenTitleHas Mutated state");
            story.Scenarios.Single().Steps.ElementAt(4).Title.ShouldBe("And SomethingWithArg foo");
            story.Scenarios.Single().Steps.ElementAt(5).Title.ShouldBe("And SomethingWithArg2 foo2");
            story.Scenarios.Single().Steps.ElementAt(6).Title.ShouldBe("And SomethingWithArg3 foo3");
        }
        [Fact]
        public void TitleFunctionCanBeOverridenWithinTestAndUseParameters()
        {
            GivenWeMutateSomeState();

            FooClass something = new FooClass();
            var context = TestContext.GetContext(something);
            Configurator.Scanners.SetDefaultStepTitleCreatorFunction((a, b, c, d, e) => new StepTitle(e + " " + c.Name + " " + string.Join(",", d.Select(arg => arg.Value).ToArray())));
            new FluentStepBuilder<FooClass>(something);
            var story = something
               .Given(_ => GivenWeMutateSomeState())
                .When(_ => something.Sub.SomethingHappens())
                .And(_ => something.Sub.SomethingWithDifferentTitle())
                .Then(_ => ThenTitleHas(AMethodCall()))
                .And(_ => something.Sub.SomethingWithArg("foo"))
                .And(_ => something.Sub.SomethingWithArg2("foo2"))
                .And(_ => something.Sub.SomethingWithArg3("foo3"))
                .BDDfy();

            story.Scenarios.Single().Steps.ElementAt(0).Title.ShouldBe("Given GivenWeMutateSomeState ");
            story.Scenarios.Single().Steps.ElementAt(1).Title.ShouldBe("When SomethingHappens ");
            story.Scenarios.Single().Steps.ElementAt(2).Title.ShouldBe("And SomethingWithDifferentTitle ");
            story.Scenarios.Single().Steps.ElementAt(3).Title.ShouldBe("Then ThenTitleHas Mutated state");
            story.Scenarios.Single().Steps.ElementAt(4).Title.ShouldBe("And SomethingWithArg foo");
            story.Scenarios.Single().Steps.ElementAt(5).Title.ShouldBe("And SomethingWithArg2 foo2");
            story.Scenarios.Single().Steps.ElementAt(6).Title.ShouldBe("And SomethingWithArg3 foo3");
        }

        [Fact]
        public void TitleFunctionCanBeOverridenFromThestartWithinTestAndUseParameters()
        {
            GivenWeMutateSomeState();

            FooClass something = new FooClass();
            var context = TestContext.GetContext(something);
            new FluentStepBuilder<FooClass>(something);
            var story = something
                .SetStepTitleFunction((a, b, c, d, e) => new StepTitle("hello"))
                .Given(_ => GivenWeMutateSomeState())
                 .When(_ => something.Sub.SomethingHappens())
                 .BDDfy();

            story.Scenarios.Single().Steps.ElementAt(0).Title.ShouldBe("hello");
            story.Scenarios.Single().Steps.ElementAt(1).Title.ShouldBe("hello");
      
           
            var story2 = something
               .Given(_ => GivenWeMutateSomeState())
               .BDDfy();

            story2.Scenarios.Single().Steps.ElementAt(0).Title.ShouldBe("Given we mutate some state");

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
            public void SomethingWithArg(string arg)
            {
            }

            [StepTitle("With arg", false)]
            public void SomethingWithArg2(string arg)
            {
            }

            [StepTitle("With {0} arg", false)]
            public void SomethingWithArg3(string arg)
            {
            }
        }

        private string AMethodCall()
        {
            return _mutatedState;
        }

        private void GivenWeMutateSomeState()
        {
            _mutatedState = "Mutated state";
        }

        private void ThenTitleHas(string result)
        {
            result.ShouldBe(_mutatedState);
        }

        public void Dispose()
        {
            Configurator.Scanners.SetDefaultStepTitleCreatorFunction(null);

        }
    }
}