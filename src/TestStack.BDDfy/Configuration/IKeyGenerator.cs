namespace TestStack.BDDfy.Configuration
{
    public interface IKeyGenerator
    {
        string GetScenarioId();
        string GetStepId();
        void Reset();
    }
}