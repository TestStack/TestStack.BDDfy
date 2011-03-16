using Bddify.Scanners;
using NUnit.Framework;

namespace SutBehaviors
{
    public class SimpleMathContext
    {
        int _result;

        [WithArgs(1, 2)]
        void GivenTwoNumbers(int input1, int input2)
        {
            _result = input1 + input2;    
        }

        void ThenTheResultIsCorrect()
        {
            Assert.That(_result, Is.EqualTo(3));
        }

        [Test]
        public  void Execute()
        {
            this.Bddify<GwtScanner>();
        }
    }
}