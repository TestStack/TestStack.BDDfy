namespace TestStack.BDDfy
{
    public class ThenAttribute : ExecutableAttribute
    {
        public ThenAttribute(string? stepTitle = null) : base(ExecutionOrder.Assertion, stepTitle)
        {
            Asserts = true;
        }
    }
}