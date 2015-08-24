namespace TestStack.BDDfy
{
    public class WhenAttribute : ExecutableAttribute
    {
        public WhenAttribute() : this(null) { }
        public WhenAttribute(string stepTitle) : base(ExecutionOrder.Transition, stepTitle) { }
    }
}