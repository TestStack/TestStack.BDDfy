using NUnit.Framework;

namespace TestStack.BDDfy.Tests.Scanner.FluentScanner
{
    class UseExamples
    {
        private int _start;
        private int _eat;

        [Test]
        public void RunExamplesWithFluentApi()
        {
            this.Given(_ => _.GivenThereAre__start__Cucumbers(Args.From<int>("Start")))
                .When(_ => _.WhenIEat__eat__Cucumbers(Args.From<int>("Eat")))
                .Then(_ => _.ThenIShouldHave__left__Cucumbers(Args.From<int>("Left")))
                .WithExamples(
                    new object[] { "Start", "Eat", "Left" },
                    new object[] { 12, 5, 8 },
                    new object[] { 20, 5, 17 })
                .BDDfy();
        }

        [Test]
        public void RunExamplesWithReflectiveApi()
        {
            this.WithExamples(
                    new object[] { "Start", "Eat", "Left" },
                    new object[] { 12, 5, 8 },
                    new object[] { 20, 5, 17 })
                .BDDfy();
        }

        private void GivenThereAre__start__Cucumbers(int start)
        {
            _start = start;
        }

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
