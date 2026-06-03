namespace TestStack.BDDfy
{
    public interface IScenarioScanner
    {
        IEnumerable<Scenario> Scan(ITestContext testContext);
    }
}