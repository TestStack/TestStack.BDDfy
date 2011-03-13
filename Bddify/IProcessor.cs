namespace Bddify
{
    public interface IProcessor
    {
        ProcessType ProcessType { get; }
        void Process(Bddee bddee);
    }
}