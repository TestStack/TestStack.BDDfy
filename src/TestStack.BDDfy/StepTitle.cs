using System;

namespace TestStack.BDDfy
{
    public class StepTitle
    {
        private readonly Func<string> _createTitle;
        private string _title;

        public StepTitle(string title) => _createTitle = () => _title = title;

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
            return _title ??= _createTitle();
        }
    }
}