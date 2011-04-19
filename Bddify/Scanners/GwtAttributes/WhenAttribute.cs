namespace Bddify.Scanners.GwtAttributes
{
    public class WhenAttribute : GwtExectuableAttribute
    {
        public WhenAttribute() : base(Core.ExecutionOrder.Transition) { }
    }
}