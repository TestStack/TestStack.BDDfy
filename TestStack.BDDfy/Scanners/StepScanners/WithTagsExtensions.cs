namespace TestStack.BDDfy
{
    public static class WithTagsExtensions
    {
        public static TScenario WithTags<TScenario>(this TScenario testObject, params string[] tags)
            where TScenario : class
        {
            var testContext = TestContext.GetContext(testObject);
            testContext.Tags.AddRange(tags);

            return testObject;
        }
    }
}