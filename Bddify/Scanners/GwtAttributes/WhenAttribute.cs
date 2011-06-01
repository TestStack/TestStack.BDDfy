namespace Bddify.Scanners.GwtAttributes
{
    public class WhenAttribute : ExecutableAttribute
    {
        public WhenAttribute() : base(Core.ExecutionOrder.Transition) { }
    }
}