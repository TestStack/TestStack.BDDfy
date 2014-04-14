using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TestStack.BDDfy
{
    public class Example : IEnumerable<ExampleValue>
    {
        private readonly ExampleValue[] _items;

        public Example(params ExampleValue[] items)
        {
            _items = items;
        }

        public string[] Headers { get { return _items.Select(i => i.Header).ToArray(); } }

        public object GetExampleValue(int index, Type targetType)
        {
            return _items[index].GetExampleValue(targetType);
        }

        public IEnumerator<ExampleValue> GetEnumerator()
        {
            return ((ICollection<ExampleValue>)_items).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            return string.Join(", ", _items.Select(i => i.ToString()));
        }
    }
}