namespace TestStack.BDDfy
{
    public interface IProcessor
    {
        ProcessType ProcessType { get; }
        void Process(Story story);
    }
}