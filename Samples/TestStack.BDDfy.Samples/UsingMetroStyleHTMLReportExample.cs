using System;
using NUnit.Framework;
using TestStack.BDDfy.Configuration;

namespace TestStack.BDDfy.Samples
{
    [TestFixture]
    [Story(AsA = "Newcomer",
        IWant = "To output a metro styled report",
        SoThat = "I can see it")]
    public class UsingMetroStyleHtmlReportExample
    {
        public int Start { get; set; }
        public int Eat { get; set; }
        public int Left { get; set; }

        [Test]
        public void RunExamplesWithFluentApi()
        {
           Configurator.BatchProcessors.HtmlReport.Disable();
           Configurator.BatchProcessors.HtmlMetroReport.Enable();

            this.Given("Given there are <start> cucumbers")
                    .And(_ => _.AndIStealTwoMore())
                .When(_ => _.WhenIEat__eat__Cucumbers())
                .Then(_ => _.ThenIShouldHave__left__Cucumbers())
                .WithExamples(new ExampleTable("Start", "Eat", "Left")
                {
                    {12, 5, 9},
                    {20, 5, 17},
                    {20, 5, -1},
                })
                .BDDfy();
        }


        [Test]
        public void RunInconclusiveStep()
        {
            Configurator.BatchProcessors.HtmlReport.Disable();
            Configurator.BatchProcessors.HtmlMetroReport.Enable();

            this.Given(_ => IDontLikeCucumbers())
                .When(_ => IEatSome())
                .Then(_ => ThenIShouldNotFeelGood())
                .BDDfy();
        }


        [Test]
        public void RunPassing()
        {
            Configurator.BatchProcessors.HtmlReport.Disable();
            Configurator.BatchProcessors.HtmlMetroReport.Enable();

            this.Given(_ => ILikeCucumbers())
                .When(_ => IEatSome())
                .Then(_ => ThenIShouldFeelGood())
                .BDDfy();
        }


        [Test]
        public void RunNotImplementedStep()
        {
            Configurator.BatchProcessors.HtmlReport.Disable();
            Configurator.BatchProcessors.HtmlMetroReport.Enable();

            this.Given(_ => IDontLikeCucumbers())
                .When(_ => IEatSome())
                .Then(_ => ThenIShouldNotFeelGoodNotImpl())
                .BDDfy();
        }


        private void IDontLikeCucumbers()
        {           
        }

        private void ILikeCucumbers()
        {
        }

        private void IEatSome()
        {
        }


        private void ThenIShouldNotFeelGood()
        {
            Assert.Inconclusive("Sample of what an inconclusive step looks like in a report");
        }

        private void ThenIShouldFeelGood()
        {
         
        }

        private void ThenIShouldNotFeelGoodNotImpl()
        {
            throw new NotImplementedException("Example not implemented ex");         
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
            if (Left == -1)
            {
                Assert.Fail("pretend there's a fail so can see what it looks like on report");
            }

            Assert.That(Start - Eat, Is.EqualTo(Left));
        }
    }
}
