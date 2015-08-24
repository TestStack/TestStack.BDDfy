namespace TestStack.BDDfy
{
    public class AndWhenAttribute : ExecutableAttribute
    {
        public AndWhenAttribute() : this(null) { }
        public AndWhenAttribute(string stepTitle) : base(ExecutionOrder.ConsecutiveTransition, stepTitle) { }
    }
}