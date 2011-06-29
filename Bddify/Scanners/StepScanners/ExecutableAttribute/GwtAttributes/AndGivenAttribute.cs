namespace Bddify.Scanners.StepScanners.ExecutableAttribute.GwtAttributes
{
    public class AndGivenAttribute : ExecutableAttribute
    {
        public AndGivenAttribute() : base(Core.ExecutionOrder.ConsecutiveSetupState) { }
    }
}