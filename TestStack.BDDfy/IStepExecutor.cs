namespace TestStack.BDDfy
{
    public interface IStepExecutor
    {
        object Execute(Step step, object testObject);
    }
}