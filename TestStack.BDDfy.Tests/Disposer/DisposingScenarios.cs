using System;
using System.Linq;
using NUnit.Framework;
using TestStack.BDDfy.Tests.Exceptions;

namespace TestStack.BDDfy.Tests.Disposer
{
    [TestFixture]
    public class DisposingScenarios
    {
        class DisposableScenario : IDisposable
        {
            private readonly bool _givenThrows;
            private readonly bool _whenThrows;
            private readonly bool _thenThrows;

            public DisposableScenario(ThrowingMethods throwingMethods)
            {
                _givenThrows = (throwingMethods & ThrowingMethods.Given) > 0;
                _whenThrows = (throwingMethods & ThrowingMethods.When) > 0;
                _thenThrows = (throwingMethods & ThrowingMethods.Then) > 0;
            }

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

        [Test]
        [TestCase(ThrowingMethods.None)]
        [TestCase(ThrowingMethods.Given)]
        [TestCase(ThrowingMethods.When)]
        [TestCase(ThrowingMethods.Then)]
        [TestCase(ThrowingMethods.Given | ThrowingMethods.When | ThrowingMethods.Then)]
        public void Execute(ThrowingMethods throwingMethods)
        {
            var scenario = new DisposableScenario(throwingMethods);

            var bddifier = scenario.LazyBDDfy();
            try
            {
                // we need TestObject for this test so have to disable StoryCache processor for this one test
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

            Assert.That(story.Scenarios.All(s => ((DisposableScenario)s.TestObject).Disposed), Is.False);
            Assert.That(scenario.Disposed, Is.False);
        }
    }
}