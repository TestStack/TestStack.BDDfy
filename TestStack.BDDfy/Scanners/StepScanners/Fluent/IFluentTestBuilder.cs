namespace TestStack.BDDfy
{
    public interface IFluentTestBuilder<T>
    {
        T TestObject { get; }
    }

    interface IFluentTestBuilder
    {
        object TestObject { get; }
    }
}