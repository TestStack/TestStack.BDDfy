namespace Bddify.Scanners.GwtAttributes
{
    public class ThenAttribute : GwtExectuableAttribute
    {
        public ThenAttribute() : base(Core.ExecutionOrder.Assertion)
        {
            Asserts = true;
        }
    }
}