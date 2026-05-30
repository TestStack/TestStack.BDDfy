using System;
using System.Collections.Generic;
using System.Linq;

namespace TestStack.BDDfy
{
    public class Example(params ExampleValue[] items)
    {
        public string[] Headers { get { return [.. Values.Select(i => i.Header)]; } }

        public IEnumerable<ExampleValue> Values { get; } = items;

        public object? GetValueOf(int index, Type targetType) => Values.ElementAt(index).GetValue(targetType);

        public override string ToString() => string.Join(", ", Values.Select(i => i.ToString()));
    }
}