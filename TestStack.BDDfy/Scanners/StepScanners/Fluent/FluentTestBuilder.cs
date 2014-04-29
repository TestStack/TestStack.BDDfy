namespace TestStack.BDDfy
{
    public class FluentTestBuilder<T> : IFluentTestBuilder<T>, IFluentTestBuilder
    {
        public FluentTestBuilder(T testObject)
        {
            TestObject = testObject;
        }

        public T TestObject { get; private set; }

        object IFluentTestBuilder.TestObject
        {
            get
            {
                return TestObject;
            }
        }
    }
}