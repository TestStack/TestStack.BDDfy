using System;
using Bddify.Core;
using NUnit.Framework;
using System.Linq;

namespace Bddify.Tests.Disposer
{
    public class DisposingScenarios
    {
        private DisposableScenario _scenario;
        private Core.Story _story;

        [RunScenarioWithArgs(false, false, false)]
        [RunScenarioWithArgs(true, false, false)]
        [RunScenarioWithArgs(false, true, false)]
        [RunScenarioWithArgs(false, false, true)]
        [RunScenarioWithArgs(true, true, true)]
        class DisposableScenario : IDisposable
        {
            private bool _givenThrows;
            private bool _whenThrows;
            private bool _thenThrows;

            void RunScenarioWithArgs(bool givenThrows, bool whenThrows, bool thenThrows)
            {
                _givenThrows = givenThrows;
                _whenThrows = whenThrows;
                _thenThrows = thenThrows;
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
            _scenario = new DisposableScenario();
        }

        void WhenScenarioIsBddified()
        {
            var bddifier = _scenario.LazyBddify();
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
        public void Execute()
        {
            this.Bddify();
        }
    }
}