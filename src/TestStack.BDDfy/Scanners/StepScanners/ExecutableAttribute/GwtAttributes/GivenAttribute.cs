namespace TestStack.BDDfy
{
    public class GivenAttribute(string stepTitle): ExecutableAttribute(ExecutionOrder.SetupState, stepTitle)
    {
        public GivenAttribute() : this(null) { }
    }
}