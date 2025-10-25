namespace TestStack.BDDfy
{
    public class ButWhenAttribute(string stepTitle): ExecutableAttribute(ExecutionOrder.ConsecutiveTransition, stepTitle)
    {
        public ButWhenAttribute() : this(null) { }
    }
}