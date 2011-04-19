namespace Bddify.Scanners.GwtAttributes
{
    public class GivenAttribute : GwtExectuableAttribute
    {
        public GivenAttribute() : base(Core.ExecutionOrder.SetupState) { }
    }
}