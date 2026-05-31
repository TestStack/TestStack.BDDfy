namespace TestStack.BDDfy
{
    public class WhenAttribute(string? stepTitle = null): ExecutableAttribute(ExecutionOrder.Transition, stepTitle)
    {
    }
}