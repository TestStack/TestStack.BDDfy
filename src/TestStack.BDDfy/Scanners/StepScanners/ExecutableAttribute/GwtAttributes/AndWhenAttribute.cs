namespace TestStack.BDDfy
{
    public class AndWhenAttribute(string stepTitle): ExecutableAttribute(ExecutionOrder.ConsecutiveTransition, stepTitle)
    {
        public AndWhenAttribute() : this(null) { }
    }
}