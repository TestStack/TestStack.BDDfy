namespace TestStack.BDDfy
{
    public interface ITestContext
    {
        object TestObject { get; }

        ExampleTable Examples { get; set; }
    }
}