namespace TestStack.BDDfy
{
    public class ButAttribute : ExecutableAttribute
    {
        public ButAttribute(string? stepTitle = null) : base(ExecutionOrder.ConsecutiveAssertion, stepTitle)
        {
            Asserts = true;
        }
    }
}