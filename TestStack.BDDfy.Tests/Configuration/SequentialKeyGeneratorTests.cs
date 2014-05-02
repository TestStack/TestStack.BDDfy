using NUnit.Framework;
using TestStack.BDDfy.Configuration;

namespace TestStack.BDDfy.Tests.Configuration
{
    [TestFixture]
    public class SequentialKeyGeneratorTests
    {
        [Test]
        public void ShouldReturnOneForFirstScenario()
        {
            Assert.That(new SequentialKeyGenerator().GetScenarioId(), Is.EqualTo("scenario-1"));
        }

        [Test]
        public void ShouldIncrementScenarioIdForEachRequestForScenariId()
        {
            var sut = new SequentialKeyGenerator();

            for (int i = 1; i <= 12; i++)
            {
                Assert.That(sut.GetScenarioId(), Is.EqualTo("scenario-" + i));
            }
        }

        [Test]
        public void ShouldReturnOneOneForFirstStepOfFirstScenario()
        {
            Assert.That(new SequentialKeyGenerator().GetStepId(), Is.EqualTo("step-1-1"));
        }

        [Test]
        public void ShouldIncrementStepIdForEachRequestForStepId()
        {
            var sut = new SequentialKeyGenerator();

            for (int i = 1; i <= 12; i++)
            {
                Assert.That(sut.GetStepId(), Is.EqualTo("step-1-" + i));
            }
        }

        [Test]
        public void ShouldResetStepCountForNewScenario()
        {
            var sut = new SequentialKeyGenerator();
            sut.GetStepId();
            sut.GetScenarioId();

            Assert.That(sut.GetStepId(), Is.EqualTo("step-2-1"));
        }
    }
}
