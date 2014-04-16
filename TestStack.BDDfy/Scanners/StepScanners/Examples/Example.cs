using System;
using System.Collections.Generic;
using System.Linq;

namespace TestStack.BDDfy
{
    public class Example
    {
        private readonly ExampleValue[] _items;

        public Example(params ExampleValue[] items)
        {
            _items = items;
        }

        public string[] Headers { get { return Values.Select(i => i.Header).ToArray(); } }

        public IEnumerable<ExampleValue> Values
        {
            get { return _items; }
        }

        public object GetValueOf(int index, Type targetType)
        {
            return _items[index].GetValue(targetType);
        }

        public override string ToString()
        {
            return string.Join(", ", Values.Select(i => i.ToString()));
        }
    }
}