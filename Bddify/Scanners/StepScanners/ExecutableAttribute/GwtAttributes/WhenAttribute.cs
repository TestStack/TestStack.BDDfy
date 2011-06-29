namespace Bddify.Scanners.StepScanners.ExecutableAttribute.GwtAttributes
{
    public class WhenAttribute : ExecutableAttribute
    {
        public WhenAttribute() : base(Core.ExecutionOrder.Transition) { }
    }
}