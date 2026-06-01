using Xunit;

namespace SampleStories
{
    public class LameExternalStory
    {
        int state = 0;
        public void GivenANumberThree() => state = 3;
        public void WhenIMultiplyTheNumberByItsef() => state *= state;
        public void ThenTheNumberIsSquared() => Assert.Equal(9, state);
    }
}
