namespace TestStack.BDDfy.Scanners.StepScanners.ExecutableAttribute.GwtAttributes
{
    public class AndGivenAttribute : ExecutableAttribute
    {
        public AndGivenAttribute() : this(null) { }
        public AndGivenAttribute(string stepTitle) : base(Core.ExecutionOrder.ConsecutiveSetupState, stepTitle) { }
    }
}