namespace TestStack.BDDfy.Scanners
{
    public class GivenAttribute : ExecutableAttribute
    {
        public GivenAttribute() : this(null) { }
        public GivenAttribute(string stepTitle) : base(Core.ExecutionOrder.SetupState, stepTitle) { }
    }
}