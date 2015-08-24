namespace TestStack.BDDfy
{
    public class ButGivenAttribute : ExecutableAttribute
    {
        public ButGivenAttribute() : this(null) { }

        public ButGivenAttribute(string stepTitle)
            : base(ExecutionOrder.ConsecutiveSetupState, stepTitle)
        {
        }
    }
}