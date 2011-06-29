using System;
using Bddify.Tests.Exceptions;
using NUnit.Framework;
using System.Linq;

namespace Bddify.Tests.Disposer
{
    [TestFixture]
    public class DisposingScenarios
    {
        private DisposableScenario _scenario;
        private Core.Story _story;
        private ThrowingMethods _throwingMethods;

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

        void GivenADisposableScenario()
        {
            _scenario = new DisposableScenario(_throwingMethods);
        }

        void WhenScenarioIsBddified()
        {
            var bddifier = _scenario.LazyBddify(true);
            try
            {
                bddifier.Run();
            }
            catch (Exception)
            {
                // there will be an exception but we do not care about it
            }
            _story = bddifier.Story;
        }

        void ThenScenariosAreNotDisposedByBddifyBecauseTheyShouldBeThereForHtmlReport()
        {
            Assert.That(_story.Scenarios.All(s => ((DisposableScenario)s.TestObject).Disposed), Is.False);
        }

        void AndTheScenarioCreatedByTestingFrameworkIsNotDisposedOf()
        {
            Assert.That(_scenario.Disposed, Is.False);
        }

        [Test]
        [TestCase(ThrowingMethods.None)]
        [TestCase(ThrowingMethods.Given)]
        [TestCase(ThrowingMethods.When)]
        [TestCase(ThrowingMethods.Then)]
        [TestCase(ThrowingMethods.Given | ThrowingMethods.When | ThrowingMethods.Then)]
        public void Execute(ThrowingMethods throwingMethods)
        {
            _throwingMethods = throwingMethods;
            this.Bddify();
        }
    }
}