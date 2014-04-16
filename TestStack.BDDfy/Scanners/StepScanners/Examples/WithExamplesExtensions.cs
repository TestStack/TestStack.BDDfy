namespace TestStack.BDDfy
{
    public static class WithExamplesExtensions
    {
        public static ITestContext WithExamples(this object testObject, ExampleTable table)
        {
            var testContext = testObject as ITestContext ?? new TestContext(testObject);
            testContext.Examples = table;

            return testContext;
        }

        public static ITestContext WithExamples(this object testObject, string table)
        {
            return testObject.WithExamples(ExampleTable.Parse(table));
        }
    }
}
