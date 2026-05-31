namespace TestStack.BDDfy
{
    public class AndWhenAttribute(string? stepTitle = null): ExecutableAttribute(ExecutionOrder.ConsecutiveTransition, stepTitle)
    {
    }
}