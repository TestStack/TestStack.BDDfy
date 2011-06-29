namespace Bddify.Scanners.StepScanners.ExecutableAttribute.GwtAttributes
{
    public class AndThenAttribute : ExecutableAttribute
    {
        public AndThenAttribute() : base(Core.ExecutionOrder.ConsecutiveAssertion)
        {
            Asserts = true;
        }
    }
}