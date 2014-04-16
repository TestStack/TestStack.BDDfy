namespace TestStack.BDDfy
{
    public class TestContext : ITestContext
    {
        public TestContext(object testObject)
        {
            TestObject = testObject;
        }

        public object TestObject { get; private set; }
        public ExampleTable Examples { get; set; }
    }
}