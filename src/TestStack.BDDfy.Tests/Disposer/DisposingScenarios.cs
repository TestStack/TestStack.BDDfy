using System;
using System.Linq;
using Shouldly;
using TestStack.BDDfy.Tests.Exceptions;
using Xunit;
using Xunit.Extensions;

namespace TestStack.BDDfy.Tests.Disposer
{
    public class DisposingScenarios
    {
        class DisposableScenario(ThrowingMethods throwingMethods): IDisposable
        {
            private readonly bool _givenThrows = (throwingMethods & ThrowingMethods.Given) > 0;
            private readonly bool _whenThrows = (throwingMethods & ThrowingMethods.When) > 0;
            private readonly bool _thenThrows = (throwingMethods & ThrowingMethods.Then) > 0;

            public void Given()
            {
                if (_givenThrows)
                    throw new Exception();
            }

            public void When()
            {
                if (_whenThrows)
                    throw new Exception();
            }

            public void Then()
            {
                if (_thenThrows)
                    throw new Exception();
            }

            public void Dispose() { Disposed = true; }

            public bool Disposed { get; set; }
        }

        [Theory]
        [InlineData(ThrowingMethods.None)]
        [InlineData(ThrowingMethods.Given)]
        [InlineData(ThrowingMethods.When)]
        [InlineData(ThrowingMethods.Then)]
        [InlineData(ThrowingMethods.Given | ThrowingMethods.When | ThrowingMethods.Then)]
        public void Execute(ThrowingMethods throwingMethods)
        {
            var scenario = new DisposableScenario(throwingMethods);

            var bddifier = scenario.LazyBDDfy();
            try
            {
                // we need TestObject for this test; so I disable StoryCache processor for this one test
                BDDfy.Configuration.Configurator.Processors.StoryCache.Disable();
                bddifier.Run();
            }
            catch (Exception)
            {
                // there will be an exception but we do not care about it
            }
            finally
            {
                BDDfy.Configuration.Configurator.Processors.StoryCache.Enable();
            }
            var story = bddifier.Story;

            story.Scenarios.All(s => ((DisposableScenario)s.TestObject).Disposed).ShouldBe(false);
            scenario.Disposed.ShouldBe(false);
        }
    }
}