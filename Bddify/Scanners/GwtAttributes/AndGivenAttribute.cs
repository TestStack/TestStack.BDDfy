namespace Bddify.Scanners.GwtAttributes
{
    public class AndGivenAttribute : ExecutableAttribute
    {
        public AndGivenAttribute() : base(Core.ExecutionOrder.ConsecutiveSetupState) { }
    }
}