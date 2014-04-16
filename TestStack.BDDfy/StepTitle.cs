namespace TestStack.BDDfy
{
    using System;

    public class StepTitle
    {
        private readonly Func<string> _createTitle;

        public StepTitle(string title)
        {
            this._createTitle = () => title;
        }

        public StepTitle(Func<string> createTitle)
        {
            this._createTitle = createTitle;
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