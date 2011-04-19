namespace Bddify.Scanners.GwtAttributes
{
    public class AndThenAttribute : GwtExectuableAttribute
    {
        public AndThenAttribute() : base(Core.ExecutionOrder.ConsequentAssertion)
        {
            Asserts = true;
        }
    }
}