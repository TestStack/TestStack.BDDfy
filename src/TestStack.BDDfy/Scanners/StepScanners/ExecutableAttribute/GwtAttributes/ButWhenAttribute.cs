namespace TestStack.BDDfy
{
    public class ButWhenAttribute : ExecutableAttribute
    {
        public ButWhenAttribute() : this(null) { }

        public ButWhenAttribute(string stepTitle)
            : base(ExecutionOrder.ConsecutiveTransition, stepTitle)
        {
        }
    }
}