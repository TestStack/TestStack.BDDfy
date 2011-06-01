namespace Bddify.Scanners.GwtAttributes
{
    public class GivenAttribute : ExecutableAttribute
    {
        public GivenAttribute() : base(Core.ExecutionOrder.SetupState) { }
    }
}