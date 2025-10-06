namespace TestStack.BDDfy
{
    public class ButGivenAttribute(string stepTitle): ExecutableAttribute(ExecutionOrder.ConsecutiveSetupState, stepTitle)
    {
        public ButGivenAttribute() : this(null) { }
    }
}