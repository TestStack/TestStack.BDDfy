using System.Collections.Generic;

namespace TestStack.BDDfy
{
    public class TestContext : ITestContext
    {
        private static readonly Dictionary<object, ITestContext> ContextLookup = new Dictionary<object, ITestContext>();

        private TestContext(object testObject)
        {
            TestObject = testObject;
            Tags = new List<string>();
        }

        public static void SetContext(object testObject, ITestContext context)
        {
            var fluentBuilder = testObject as IFluentStepBuilder;
            if (fluentBuilder != null) testObject = fluentBuilder.TestObject;

            if (ContextLookup.ContainsKey(testObject))
            {
                var oldContext = ContextLookup[testObject];
                context.Examples = oldContext.Examples;
                ContextLookup[testObject] = new TestContext(testObject);
            }
            else
            {
                ContextLookup.Add(testObject, context);
            }
        }

        public static ITestContext GetContext(object testObject)
        {
            var fluentBuilder = testObject as IFluentStepBuilder;
            if (fluentBuilder != null) testObject = fluentBuilder.TestObject;

            if (!ContextLookup.ContainsKey(testObject))
                ContextLookup.Add(testObject, new TestContext(testObject));

            return ContextLookup[testObject];
        }

        public static void ClearContextFor(object testObject)
        {
            if (ContextLookup.ContainsKey(testObject))
                ContextLookup.Remove(testObject);
        }

        public ExampleTable Examples { get; set; }
        public IFluentScanner FluentScanner { get; set; }
        public List<string> Tags { get; private set; }
        public object TestObject { get; private set; }
    }
}