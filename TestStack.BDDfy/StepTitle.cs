namespace TestStack.BDDfy
{
    public class StepTitle
    {
        private readonly string _title;

        public StepTitle(string title)
        {
            this._title = title;
        }

        public static implicit operator string(StepTitle title)
        {
            return title.ToString();
        }

        public override string ToString()
        {
            return _title;
        }
    }
}