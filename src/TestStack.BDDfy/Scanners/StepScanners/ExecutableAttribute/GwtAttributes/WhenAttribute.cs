namespace TestStack.BDDfy
{
    public class WhenAttribute(string stepTitle): ExecutableAttribute(ExecutionOrder.Transition, stepTitle)
    {
        public WhenAttribute() : this(null) { }
    }
}