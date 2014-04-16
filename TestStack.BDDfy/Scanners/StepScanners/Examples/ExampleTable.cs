using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TestStack.BDDfy
{
    public class ExampleTable : ICollection<Example>
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

        public void Add(params object[] items)
        {
            if (items.Length != Headers.Length)
                throw new ArgumentException(string.Format("Number of column values does not match number of headers, got {0}, expected {1}", items.Length, Headers.Length));

            Example example = null;
            // ReSharper disable once AccessToModifiedClosure
            example = new Example(items.Select((o, i) => new ExampleValue(Headers[i], o, () => _rows.IndexOf(example) + 1)).ToArray());
            Add(example);
        }

        public void Add(Example example)
        {
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

        public static ExampleTable Parse(string table)
        {
            var lines = table.Trim().Split('\n').Select(l => l.Trim().Trim('|')).ToArray();
            var firstRow = lines.First();
            var headers = firstRow.Split('|').Select(h => h.Trim()).ToArray();

            var exampleTable = new ExampleTable(headers);

            foreach (var line in lines.Skip(1))
                exampleTable.Add(GetValues(line));

            return exampleTable;
        }

        static object[] GetValues(string row)
        {
            return row.Split('|').Select(h => (object)h.Trim()).ToArray();
        }
    }
}