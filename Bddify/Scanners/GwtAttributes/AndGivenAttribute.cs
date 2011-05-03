namespace Bddify.Scanners.GwtAttributes
{
    public class AndGivenAttribute : GwtExectuableAttribute
    {
        public AndGivenAttribute() : base(Core.ExecutionOrder.ConsecutiveSetupState) { }
    }
}