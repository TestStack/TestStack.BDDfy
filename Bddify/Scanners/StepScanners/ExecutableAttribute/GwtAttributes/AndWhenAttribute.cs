namespace Bddify.Scanners.StepScanners.ExecutableAttribute.GwtAttributes
{
    public class AndWhenAttribute : ExecutableAttribute
    {
        public AndWhenAttribute() : base(Core.ExecutionOrder.ConsecutiveTransition) { }
    }
}