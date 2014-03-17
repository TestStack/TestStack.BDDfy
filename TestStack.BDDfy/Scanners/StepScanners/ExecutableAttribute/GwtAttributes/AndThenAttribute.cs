namespace TestStack.BDDfy
{
    public class AndThenAttribute : ExecutableAttribute
    {
        public AndThenAttribute() : this(null) { }

        public AndThenAttribute(string stepTitle) : base(ExecutionOrder.ConsecutiveAssertion, stepTitle)
        {
            Asserts = true;
        }
    }
}