namespace TestStack.BDDfy
{
    public class AndGivenAttribute(string stepTitle): ExecutableAttribute(ExecutionOrder.ConsecutiveSetupState, stepTitle)
    {
        public AndGivenAttribute() : this(null) { }
    }
}