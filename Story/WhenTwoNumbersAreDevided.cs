using NUnit.Framework;

namespace StoryDemo
{
    public class WhenTwoNumbersAreDevided
    {
        void ThenTheResultIsCorrect()
        {
            Assert.That(4/2, Is.EqualTo(2));
        }
    }
}