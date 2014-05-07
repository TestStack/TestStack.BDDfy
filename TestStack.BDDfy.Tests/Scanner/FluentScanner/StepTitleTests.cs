using System.Linq;
using NUnit.Framework;
using Shouldly;

namespace TestStack.BDDfy.Tests.Scanner.FluentScanner
{
    [TestFixture]
    public class StepTitleTests
    {
        [Test]
        public void MethodCallInStepTitle()
        {
            var story = this.Given(_ => GivenAValueOf(AMethodCall()))
                .BDDfy();

            story.Scenarios.Single().Steps.Single().Title.ShouldBe("Given a value of Some value");
        }

        private string AMethodCall()
        {
            return "Some value";
        }

        private void GivenAValueOf(string result)
        {
            
        }
    }
}