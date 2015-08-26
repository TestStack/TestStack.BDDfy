namespace TestStack.BDDfy
{
    public class AndGivenAttribute : ExecutableAttribute
    {
        public AndGivenAttribute() : this(null) { }
        public AndGivenAttribute(string stepTitle) : base(ExecutionOrder.ConsecutiveSetupState, stepTitle) { }
    }
}