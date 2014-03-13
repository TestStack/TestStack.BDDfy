namespace TestStack.BDDfy.Scanners
{
    public class AndWhenAttribute : ExecutableAttribute
    {
        public AndWhenAttribute() : this(null) { }
        public AndWhenAttribute(string stepTitle) : base(Core.ExecutionOrder.ConsecutiveTransition, stepTitle) { }
    }
}