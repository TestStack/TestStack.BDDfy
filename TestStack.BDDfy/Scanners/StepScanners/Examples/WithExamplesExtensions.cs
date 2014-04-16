namespace TestStack.BDDfy
{
    public static class WithExamplesExtensions
    {
        public static ITestContext<TScenario> WithExamples<TScenario>(this TScenario testObject, ExampleTable table)
            where TScenario : class
        {
            var testContext = new TestContext<TScenario>(testObject) {Examples = table};

            return testContext;
        }

        public static ITestContext<TScenario> WithExamples<TScenario>(this TScenario testObject, string table)
            where TScenario : class
        {
            return testObject.WithExamples(ExampleTable.Parse(table));
        }

        public static ITestContext WithExamples<TScenario>(this IStepsBase<TScenario> testContext, ExampleTable table)
            where TScenario : class
        {
            testContext.Examples = table;

            return testContext;
        }

        public static ITestContext WithExamples<TScenario>(this IStepsBase<TScenario> testContext, string table)
            where TScenario : class
        {
            return testContext.WithExamples(ExampleTable.Parse(table));
        }


        public static ITestContext WithExamples<TScenario>(this IWhen<TScenario> testContext, ExampleTable table)
            where TScenario : class
        {
            testContext.Examples = table;

            return testContext;
        }

        public static ITestContext WithExamples<TScenario>(this IWhen<TScenario> testContext, string table)
            where TScenario : class
        {
            return testContext.WithExamples(ExampleTable.Parse(table));
        }

        public static ITestContext WithExamples<TScenario>(this IGiven<TScenario> testContext, ExampleTable table)
            where TScenario : class
        {
            testContext.Examples = table;

            return testContext;
        }

        public static ITestContext WithExamples<TScenario>(this IGiven<TScenario> testContext, string table)
            where TScenario : class
        {
            return testContext.WithExamples(ExampleTable.Parse(table));
        }

        public static ITestContext WithExamples<TScenario>(this IThen<TScenario> testContext, ExampleTable table)
            where TScenario : class
        {
            testContext.Examples = table;

            return testContext;
        }

        public static ITestContext WithExamples<TScenario>(this IThen<TScenario> testContext, string table)
            where TScenario : class
        {
            return testContext.WithExamples(ExampleTable.Parse(table));
        }
    }
}
