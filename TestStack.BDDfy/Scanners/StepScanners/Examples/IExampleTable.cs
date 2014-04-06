using System.Collections.Generic;

namespace TestStack.BDDfy
{
    public interface IExampleTable : IEnumerable<Example>
    {
        string[] Headers { get; }
        object TestObject { get; }
        int Count { get; }
    }
}