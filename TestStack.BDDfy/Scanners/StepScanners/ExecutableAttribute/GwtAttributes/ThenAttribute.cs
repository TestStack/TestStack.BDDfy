namespace TestStack.BDDfy
{
    public class ThenAttribute : ExecutableAttribute
    {
        public ThenAttribute() : this(null) { }

        public ThenAttribute(string stepTitle) : base(ExecutionOrder.Assertion, stepTitle)
        {
            Asserts = true;
        }
    }
}