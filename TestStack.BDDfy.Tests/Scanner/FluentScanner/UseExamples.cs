using NUnit.Framework;

namespace TestStack.BDDfy.Tests.Scanner.FluentScanner
{
    class UseExamples
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
            var story = this
                .Given(_ => _.GivenThereAre__start__Cucumbers(_.Start))
                .When(_ => _.WhenIEat__eat__Cucumbers(_.Eat))
                .Then(_ => _.ThenIShouldHave__left__Cucumbers(_.Left))
                .WithExamples(
                    new[] { "Start", "Eat", "Left" },
                    new object[] { 12, 5, 8 },
                    new object[] { 20, 5, 17 })
                .BDDfy();
        }


        [Test]
        public void RunExamplesWithReflectiveApi()
        {
            this.WithExamples(
                    new [] { "start", "eat", "left" },
                    new object[] { 12, 5, 7 },
                    new object[] { 20, 5, 15 })
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
