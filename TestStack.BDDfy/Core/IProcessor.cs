namespace TestStack.BDDfy.Core
{
    public interface IProcessor
    {
        ProcessType ProcessType { get; }
        void Process(Story story);
    }
}