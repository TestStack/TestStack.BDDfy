using NUnit.Framework;

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
                .When(_ => _.WhenIEat__eat__Cucumbers())
                .Then(_ => _.ThenIShouldHave__left__Cucumbers())
                .WithExamples(new ExampleTable("Start", "Eat", "Left")
                {
                    {12, 5, 7},
                    {20, 5, 15}
                })
                .BDDfy();
        }


        // I didn't have to create this method 
        // because the inline step title has the <start> placeholder in it 
        // and I didn't have any logic more than setting the Start property
        // which is done by the framework. 
        //private void GivenThereAre__start__Cucumbers()
        //{
        //}

        private void WhenIEat__eat__Cucumbers()
        {
            // because the name contains __eat__ the Eat field/property is fetched from the examples and set by the framework per example
            // you can obviously take additional setup actions here if you want
        }

        private void ThenIShouldHave__left__Cucumbers()
        {
            Assert.That(Start - Eat, Is.EqualTo(Left));
        }
    }
}
