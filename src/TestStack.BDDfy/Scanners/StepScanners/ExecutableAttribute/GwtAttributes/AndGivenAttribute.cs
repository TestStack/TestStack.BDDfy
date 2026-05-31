namespace TestStack.BDDfy
{
    public class AndGivenAttribute(string? stepTitle = null): ExecutableAttribute(ExecutionOrder.ConsecutiveSetupState, stepTitle)
    {
    }
}