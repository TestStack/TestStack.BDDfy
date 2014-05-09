namespace TestStack.BDDfy
{
    public class ButAttribute : ExecutableAttribute
    {
        public ButAttribute() : this(null) { }

        public ButAttribute(string stepTitle) : base(ExecutionOrder.ConsecutiveAssertion, stepTitle)
        {
            Asserts = true;
        }
    }
}