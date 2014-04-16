﻿using NUnit.Framework;

namespace TestStack.BDDfy.Samples
{
    [TestFixture]
    public class UseExamplesWithFluentApi
    {
        public int Start { get; set; }
        public int Eat { get; set; }
        public int Left { get; set; }

        [Test]
        public void RunExamplesWithFluentApi()
        {
            this.Given("Given there are <start> cucumbers")
                    .And(_ => _.AndIStealTwoMore())
                .When(_ => _.WhenIEat__eat__Cucumbers())
                .Then(_ => _.ThenIShouldHave__left__Cucumbers())
                .WithExamples(new ExampleTable("Start", "Eat", "Left")
                {
                    {12, 5, 9},
                    {20, 5, 17}
                })
                .BDDfy();
        }


        // I didn't have to create this method 
        // because all it was going to do was to set Start property 
        // which is being handled by the framework
        // And the step title is provided inline
        //private void GivenThereAre__start__Cucumbers()
        //{
        //}

        private void AndIStealTwoMore()
        {
            Start += 2;
        }

        private void WhenIEat__eat__Cucumbers()
        {
            // This method is called after the Eat property is set by the framework
            // I didn't have to put this here, like the Given method, but I put it here to show that 
            // you can take additional actions here if you want
        }

        private void ThenIShouldHave__left__Cucumbers()
        {
            Assert.That(Start - Eat, Is.EqualTo(Left));
        }
    }
}
