namespace TestStack.BDDfy
{
    public class GivenAttribute : ExecutableAttribute
    {
        public GivenAttribute() : this(null) { }
        public GivenAttribute(string stepTitle) : base(ExecutionOrder.SetupState, stepTitle) { }
    }
}