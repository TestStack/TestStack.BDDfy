using System;

namespace TestStack.BDDfy
{
    public class StepTitle
    {
        private readonly Func<string> _createTitle;

        public StepTitle(string title)
        {
            _createTitle = () => title;
        }

        public StepTitle(Func<string> createTitle)
        {
            _createTitle = createTitle;
        }

        public static implicit operator string(StepTitle title)
        {
            return title.ToString();
        }

        public override string ToString()
        {
            return _createTitle();
        }
    }
}