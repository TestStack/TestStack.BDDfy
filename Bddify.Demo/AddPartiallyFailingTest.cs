using Bddify.Scanners;
using NUnit.Framework;

namespace Bddify.Demo
{
    public class AddPartiallyFailingTest
    {
        void GivenTwoNumbers()
        {
        }

        void WhenTheNumbersAreAdded()
        {
        }

        [WithArgs(1, 2, 3)]
        [WithArgs(-1, 5, 4)]
        [WithArgs(1, 3, 6)] // failure
        void ThenTheResultIsCorrect(int input1, int input2, int expectedResult)
        {
            Assert.That(input1 + input2, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Execute()
        {
            this.Bddify<GwtScanner>();
        }
    }
}