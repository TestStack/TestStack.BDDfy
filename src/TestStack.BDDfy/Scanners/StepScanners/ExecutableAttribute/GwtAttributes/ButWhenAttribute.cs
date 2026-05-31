namespace TestStack.BDDfy
{
    public class ButWhenAttribute(string? stepTitle = null): ExecutableAttribute(ExecutionOrder.ConsecutiveTransition, stepTitle)
    {
    }
}