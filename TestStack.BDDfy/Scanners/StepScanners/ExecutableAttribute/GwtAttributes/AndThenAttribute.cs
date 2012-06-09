namespace TestStack.BDDfy.Scanners.StepScanners.ExecutableAttribute.GwtAttributes
{
    public class AndThenAttribute : ExecutableAttribute
    {
        public AndThenAttribute() : this(null) { }

        public AndThenAttribute(string stepTitle) : base(Core.ExecutionOrder.ConsecutiveAssertion, stepTitle)
        {
            Asserts = true;
        }
    }
}