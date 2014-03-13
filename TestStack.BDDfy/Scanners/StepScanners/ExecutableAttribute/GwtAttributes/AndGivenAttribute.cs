namespace TestStack.BDDfy.Scanners
{
    public class AndGivenAttribute : ExecutableAttribute
    {
        public AndGivenAttribute() : this(null) { }
        public AndGivenAttribute(string stepTitle) : base(Core.ExecutionOrder.ConsecutiveSetupState, stepTitle) { }
    }
}