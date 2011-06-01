namespace Bddify.Scanners.GwtAttributes
{
    public class ThenAttribute : ExecutableAttribute
    {
        public ThenAttribute() : base(Core.ExecutionOrder.Assertion)
        {
            Asserts = true;
        }
    }
}