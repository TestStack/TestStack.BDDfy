using NUnit.Framework;
using TestStack.BDDfy.Core;
using TestStack.BDDfy.Scanners.StepScanners.Fluent;

namespace TestStack.BDDfy.Samples
{
    [Story(
        AsA = "As a .Net programmer",
        IWant = "I want to use BDDfy",
        SoThat = "So that BDD becomes easy and fun")]
    public class BDDfy_Rocks
    {
        void Given_I_have_not_used_BDDfy_before()
        {
        }

        void WhenIAmIntroducedToTheFramework()
        {
        }

        void ThenILikeItAndStartUsingIt()
        {
        }

        void AndTheQualityAndMaintainabilityOfMyTestSkyrocket()
        {
        }

        [Test]
        public void BDDfy_with_reflective_API()
        {
            this.BDDfy();
        }

        [Test]
        public void BDDfy_with_fluent_API()
        {
            this.Given(_ => Given_I_have_not_used_BDDfy_before())
                .When(_ => WhenIAmIntroducedToTheFramework())
                .Then(_ => ThenILikeItAndStartUsingIt())
                .And(_ => AndTheQualityAndMaintainabilityOfMyTestSkyrocket())
                .BDDfy();
        }
    }
}
