namespace TestStack.BDDfy.Configuration
{
    public interface IKeyGenerator
    {
        string GetScenarioId(Scenario scenario);
        string GetStepId(Step step);
        void Reset();
    }
}