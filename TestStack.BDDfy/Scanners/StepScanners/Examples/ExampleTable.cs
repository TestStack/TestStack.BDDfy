using System;
using System.Collections;
using System.Collections.Generic;

namespace TestStack.BDDfy
{
    public class ExampleTable : IExampleTable, ICollection<Example>
    {
        private readonly List<Example> _rows = new List<Example>();

        public ExampleTable(params string[] headers)
        {
            Headers = headers;
        }

        public object TestObject { get; internal set; }
        public string[] Headers { get; private set; }

        public int Count { get { return _rows.Count; } }
        public bool IsReadOnly { get { return false; } }

        public void Add(params object[] item)
        {
            Add(new Example(item));
        }

        public void Add(Example example)
        {
            if (example.ColumnCount != Headers.Length)
                throw new ArgumentException(string.Format("Example does not have the same number as columns as headers: {0}", example));

            example.Headers = Headers;
            example.ExampleIndex = _rows.Count;
            _rows.Add(example);
        }

        public void Clear()
        {
            _rows.Clear();
        }

        public bool Contains(Example item)
        {
            return _rows.Contains(item);
        }

        public void CopyTo(Example[] array, int arrayIndex)
        {
            _rows.CopyTo(array, arrayIndex);
        }

        public bool Remove(Example item)
        {
            return _rows.Remove(item);
        }

        public IEnumerator<Example> GetEnumerator()
        {
            return _rows.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}