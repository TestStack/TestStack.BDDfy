using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TestStack.BDDfy
{
    public class Example : IEnumerable<KeyValuePair<string, object>>
    {
        private readonly object[] _items;

        public Example(params object[] items)
        {
            _items = items;
        }

        public string[] Headers { get; internal set; }
        public object[] Values { get { return _items; } }
        public int ExampleIndex { get; internal set; }
        public int ColumnCount { get { return _items.Length; } }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return Headers.Select((t, index) => new KeyValuePair<string, object>(t, _items[index])).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}