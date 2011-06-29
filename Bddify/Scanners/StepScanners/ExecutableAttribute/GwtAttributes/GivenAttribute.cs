namespace Bddify.Scanners.StepScanners.ExecutableAttribute.GwtAttributes
{
    public class GivenAttribute : ExecutableAttribute
    {
        public GivenAttribute() : base(Core.ExecutionOrder.SetupState) { }
    }
}