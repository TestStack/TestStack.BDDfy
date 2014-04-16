namespace TestStack.BDDfy
{
    public interface ITestContext<TScenario> : ITestContext
    {
        
    }

    public interface ITestContext
    {
        object TestObject { get; }

        ExampleTable Examples { get; set; }
    }
}