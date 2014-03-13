namespace TestStack.BDDfy.Scanners
{
    public class WhenAttribute : ExecutableAttribute
    {
        public WhenAttribute() : this(null) { }
        public WhenAttribute(string stepTitle) : base(Core.ExecutionOrder.Transition, stepTitle) { }
    }
}