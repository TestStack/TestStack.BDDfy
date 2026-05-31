namespace TestStack.BDDfy
{
    public class GivenAttribute(string? stepTitle = null): ExecutableAttribute(ExecutionOrder.SetupState, stepTitle)
    {
    }
}