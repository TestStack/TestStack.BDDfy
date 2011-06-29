namespace Bddify.Scanners.StepScanners.ExecutableAttribute.GwtAttributes
{
    public class ThenAttribute : ExecutableAttribute
    {
        public ThenAttribute() : base(Core.ExecutionOrder.Assertion)
        {
            Asserts = true;
        }
    }
}