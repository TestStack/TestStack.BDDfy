namespace TestStack.BDDfy.Scanners.StepScanners.ExecutableAttribute.GwtAttributes
{
    public class ThenAttribute : ExecutableAttribute
    {
        public ThenAttribute() : this(null) { }

        public ThenAttribute(string stepTitle) : base(Core.ExecutionOrder.Assertion, stepTitle)
        {
            Asserts = true;
        }
    }
}