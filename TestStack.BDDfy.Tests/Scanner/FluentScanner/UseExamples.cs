using ApprovalTests;
using ApprovalTests.Reporters;
using NUnit.Framework;
using TestStack.BDDfy.Reporters;

namespace TestStack.BDDfy.Tests.Scanner.FluentScanner
{
    [TestFixture]
    public class UseExamples
    {
        //For reflective
        private int _start;
        private int _eat;

        //For Fluent
        public int Start { get; set; }
        public int Eat { get; set; }
        public int Left { get; set; }

        [Test]
        public void RunExamplesWithFluentApi()
        {
            var engine = this
                .Given(_ => _.GivenThereAre__start__Cucumbers(_.Start), false)
                .When(_ => _.WhenIEat__eat__Cucumbers(_.Eat), false)
                .Then(_ => _.ThenIShouldHave__left__Cucumbers(_.Left), false)
                .WithExamples(new ExampleTable("Start", "Eat", "Left")
                {
                    {12, 5, 7},
                    {20, 5, 17}
                })
                .LazyBDDfy();

            Assert.Throws<AssertionException>(() => engine.Run());
        }

        [Test]
        public void RunExamplesWithReflectiveApi()
        {
            this.WithExamples(new ExampleTable("start", "eat", "left")
                {
                    {12, 5, 7},
                    {20, 5, 17}
                })
                .BDDfy();
        }

        private void GivenThereAre__start__Cucumbers(int start)
        {
            _start = start;
        }

        [AndGiven("And I eat <eat> of them")]
        private void WhenIEat__eat__Cucumbers(int eat)
        {
            _eat = eat;
        }

        private void ThenIShouldHave__left__Cucumbers(int left)
        {
            Assert.That(_start - _eat, Is.EqualTo(left));
        }

        [Test]
        public void GettingTitle()
        {
            Assert.That(NetToString.Convert("GivenThereAre__start__Cucumbers"), Is.EqualTo("Given there are <start> cucumbers"));
            Assert.That(NetToString.Convert("Given_there_are__start__cucumbers"), Is.EqualTo("Given there are <start> cucumbers"));
        }
    }
}
