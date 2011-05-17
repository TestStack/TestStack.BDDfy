using Bddify.Scanners;
using NUnit.Framework;
using Bddify.Core;
using System.Linq;

namespace Bddify.Tests.FluentStepScanner
{
    public class WhenBddifyAScenarioUsingFluentApi
    {
        int[] _inputs;
        int[] _results;

        public void GivenSomeInputs(int[] inputs)
        {
            _inputs = inputs;
            _results = new int[inputs.Length];
        }

        public void WhenScenarioIsBddifiedUsingFluentStepScanner()
        {
            for (int i = 0; i < _inputs.Length; i++)
            {
                _results[i] = _inputs[i] * 2;
            }
        }

        public void ThenScenarioIsOnlyInstantiatedOnce()
        {
            for (int i = 0; i < _inputs.Length; i++)
            {
                Assert.That(_results[i], Is.EqualTo(_inputs[i] * 2));
            }
        }

        [Test]
        public void EnteringArgumentsInline()
        {
            var scanner = new FluentStepScanner<WhenBddifyAScenarioUsingFluentApi>()
                .Given(x => x.GivenSomeInputs(new[] { 1, 2 }))
                .When(x => x.WhenScenarioIsBddifiedUsingFluentStepScanner())
                .Then(x => x.ThenScenarioIsOnlyInstantiatedOnce());

            this.Bddify(scanner);
        }


    }
}