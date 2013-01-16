namespace TestStack.BDDfy.Scanners.StepScanners.ExecutableAttribute.GwtAttributes
{
    public class GivenAttribute : ExecutableAttribute
    {
        public GivenAttribute() : this(null) { }
        public GivenAttribute(string stepTitle) : base(Core.ExecutionOrder.SetupState, stepTitle) { }
    }
}