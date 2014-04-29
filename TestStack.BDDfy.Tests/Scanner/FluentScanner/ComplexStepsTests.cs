using NUnit.Framework;
using Shouldly;

namespace TestStack.BDDfy.Tests.Scanner.FluentScanner
{
    [TestFixture]
    public class ComplexStepsTests
    {
        private int count;

        [Test]
        public void ShouldBeAbleToChainComplexTestWithFluentApi()
        {
            this.Given(_ => count.ShouldBe(0))
                .When(() => count++.ShouldBe(0), "When I do something")
                .Given(() => count++.ShouldBe(1), "Given I am doing things in different order")
                .Then(() => count++.ShouldBe(2), "Then they should run in defined order")
                .When(() => count++.ShouldBe(3), "When I have whens after thens things still work")
                .And(() => count++.ShouldBe(4), "And we should still be able to use ands")
                .BDDfy();
        }
    }
}