namespace TestStack.BDDfy
{
    public static class WithExamplesExtensions
    {
        public static TScenario WithExamples<TScenario>(this TScenario testObject, ExampleTable table)
            where TScenario : class
        {
            var testContext = TestContext.GetContext(testObject);
            testContext.Examples = table;

            return testObject;
        }

        public static TScenario WithExamples<TScenario>(this TScenario testObject, string table)
            where TScenario : class
        {
            return testObject.WithExamples(ExampleTable.Parse(table));
        }
    }
}
