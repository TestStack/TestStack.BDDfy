namespace TestStack.BDDfy
{
    public class AndThenAttribute : ExecutableAttribute
    {
        public AndThenAttribute(string? stepTitle = null) : base(ExecutionOrder.ConsecutiveAssertion, stepTitle)
        {
            Asserts = true;
        }
    }
}