namespace Bddify.Scanners.GwtAttributes
{
    public class AndThenAttribute : ExecutableAttribute
    {
        public AndThenAttribute() : base(Core.ExecutionOrder.ConsecutiveAssertion)
        {
            Asserts = true;
        }
    }
}