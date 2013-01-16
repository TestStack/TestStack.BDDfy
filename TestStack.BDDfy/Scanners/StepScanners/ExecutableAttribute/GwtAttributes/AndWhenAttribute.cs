namespace TestStack.BDDfy.Scanners.StepScanners.ExecutableAttribute.GwtAttributes
{
    public class AndWhenAttribute : ExecutableAttribute
    {
        public AndWhenAttribute() : this(null) { }
        public AndWhenAttribute(string stepTitle) : base(Core.ExecutionOrder.ConsecutiveTransition, stepTitle) { }
    }
}