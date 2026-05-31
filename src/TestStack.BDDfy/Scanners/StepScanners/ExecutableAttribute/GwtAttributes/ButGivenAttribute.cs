namespace TestStack.BDDfy
{
    public class ButGivenAttribute(string? stepTitle = null): ExecutableAttribute(ExecutionOrder.ConsecutiveSetupState, stepTitle)
    {
    }
}