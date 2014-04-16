namespace TestStack.BDDfy
{
    internal class TestContext : ITestContext
    {
        public TestContext(object testObject)
        {
            TestObject = testObject;
        }

        public object TestObject { get; private set; }
        public ExampleTable Examples { get; set; }
    }

    internal class TestContext<TScenario> : TestContext, ITestContext<TScenario>
    {
        public TestContext(object testObject) : base(testObject)
        {
        }
    }
}