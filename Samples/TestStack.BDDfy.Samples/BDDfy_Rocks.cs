﻿using Xunit;

namespace TestStack.BDDfy.Samples
{
    [Story(
        AsA = "As a .Net programmer",
        IWant = "I want to use BDDfy",
        SoThat = "So that BDD becomes easy and fun",
        ImageUri = "https://upload.wikimedia.org/wikipedia/commons/7/72/DirkvdM_rocks.jpg",
        StoryUri = "https://en.wikipedia.org/wiki/Rock_%28geology%29")]
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

        [Fact]
        public void BDDfy_with_reflective_API()
        {
            this.BDDfy();
        }

        [Fact]
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
