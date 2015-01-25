using Shouldly;
using TestStack.BDDfy.Configuration;
using Xunit;

namespace TestStack.BDDfy.Tests.Configuration
{
    public class SequentialKeyGeneratorTests
    {
        [Fact]
        public void ShouldReturnOneForFirstScenario()
        {
            new SequentialKeyGenerator().GetScenarioId().ShouldBe("scenario-1");
        }

        [Fact]
        public void ShouldIncrementScenarioIdForEachRequestForScenariId()
        {
            var sut = new SequentialKeyGenerator();

            for (int i = 1; i <= 12; i++)
            {
                sut.GetScenarioId().ShouldBe("scenario-" + i);
            }
        }

        [Fact]
        public void ShouldReturnOneOneForFirstStepOfFirstScenario()
        {
            new SequentialKeyGenerator().GetStepId().ShouldBe("step-1-1");
        }

        [Fact]
        public void ShouldIncrementStepIdForEachRequestForStepId()
        {
            var sut = new SequentialKeyGenerator();

            for (int i = 1; i <= 12; i++)
            {
                sut.GetStepId().ShouldBe("step-1-" + i);
            }
        }

        [Fact]
        public void ShouldResetStepCountForNewScenario()
        {
            var sut = new SequentialKeyGenerator();
            sut.GetStepId();
            sut.GetScenarioId();

            sut.GetStepId().ShouldBe("step-2-1");
        }
    }
}
