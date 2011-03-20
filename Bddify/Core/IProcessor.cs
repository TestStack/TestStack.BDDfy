namespace Bddify.Core
{
    public interface IProcessor
    {
        ProcessType ProcessType { get; }
        void Process(Scenario scenario);
    }
}