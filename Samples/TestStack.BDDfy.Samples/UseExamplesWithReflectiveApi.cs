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
            this.Given(_ => _.GivenThereAre__start__Cucumbers())
                .When(_ => _.WhenIEat__eat__Cucumbers())
                .Then(_ => _.ThenIShouldHave__left__Cucumbers())
                .WithExamples(new ExampleTable("Start", "Eat", "Left")
                {
                    {12, 5, 7},
                    {20, 5, 15}
                })
                .BDDfy();
        }

        private void GivenThereAre__start__Cucumbers()
        {
            // because the name contains __start__ the Start field/property is fetched from the examples and set by the framework per example

            // you can obviously take additional setup actions here if you want
        }

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
