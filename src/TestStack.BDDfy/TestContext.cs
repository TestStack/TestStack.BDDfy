using System.Collections.Generic;
#if NET9_0_OR_GREATER
using System.Threading;
#endif
namespace TestStack.BDDfy
{
    public class TestContext : ITestContext
    {
        private static readonly Dictionary<object, ITestContext> ContextLookup = [];
#if NET9_0_OR_GREATER
        private static readonly Lock DictionaryLock = new();
#else
        private static readonly object DictionaryLock = new();
#endif
        private TestContext(object testObject)
        {
            TestObject = testObject;
            Tags = [];
        }

        public static void SetContext(object testObject, ITestContext context)
        {
            if (testObject is IFluentStepBuilder fluentBuilder) testObject = fluentBuilder.TestObject;
            
            lock (DictionaryLock)
            {
                if (ContextLookup.TryGetValue(testObject, out var oldContext))
                {
                    context.Examples = oldContext.Examples;
                    ContextLookup[testObject] = new TestContext(testObject);
                }
                else
                {
                    ContextLookup.Add(testObject, context);
                }
            }
        }

        public static ITestContext GetContext(object testObject)
        {
            if (testObject is IFluentStepBuilder fluentBuilder) testObject = fluentBuilder.TestObject;

            lock (DictionaryLock)
            {
                if (!ContextLookup.ContainsKey(testObject))
                    ContextLookup.Add(testObject, new TestContext(testObject));

                return ContextLookup[testObject];
            }
        }

        public static void ClearContextFor(object testObject)
        {
            lock (DictionaryLock)
            {
                ContextLookup.Remove(testObject);
            }
        }

        public ExampleTable? Examples { get; set; }
        public IFluentScanner? FluentScanner { get; set; }
        public List<string> Tags { get; private set; }
        public object TestObject { get; private set; }
    }
}