using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestStack.BDDfy
{
    public class ExampleTable : ICollection<Example>
    {
        private readonly List<Example> _rows = new List<Example>();

        public ExampleTable(params string[] headers)
        {
            Headers = headers;
        }

        public string[] Headers { get; private set; }

        public int Count { get { return _rows.Count; } }

        public bool IsReadOnly { get { return false; } }

        public void Add(params object[] items)
        {
            if (items.Length != Headers.Length)
                throw new ArgumentException(string.Format("Number of column values does not match number of headers, got {0}, expected {1}", items.Length, Headers.Length));

            Example example = null;
            // ReSharper disable once AccessToModifiedClosure
            example = new Example(items.Select((o, i) => new ExampleValue(Headers[i], o, () => _rows.IndexOf(example))).ToArray());
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
            return row.Split('|').Select(h => h.Trim()).Select(s=>string.IsNullOrEmpty(s) ? null : (object)s).ToArray();
        }

        public static bool HeaderMatches(string header, string name)
        {
            if (name == null)
                return false;

            return Sanitise(name).Equals(Sanitise(header), StringComparison.InvariantCultureIgnoreCase);
        }

        private static string Sanitise(string value)
        {
            return value.Replace(" ", string.Empty).Replace("_", string.Empty);
        }

        public string ToString(string[] additionalHeaders, string[][] additionalData)
        {
            var numberColumns = Headers.Length + additionalHeaders.Length;
            var maxWidth = new int[numberColumns];

            var rows = new List<string[]>();

            Action<IEnumerable<string>> addRow = cells =>
            {
                var row = new string[numberColumns];
                var index = 0;

                foreach (var cellText in cells)
                {
                    row[index++] = cellText;
                }

                for (var i = 0; i < numberColumns; i++)
                {
                    var rowValue = row[i];
                    if (rowValue != null && rowValue.Length > maxWidth[i])
                    {
                        maxWidth[i] = rowValue.Length;
                    }
                }

                rows.Add(row);
            };

            addRow(Headers.Concat(additionalHeaders));
            var rowIndex = 0;
            foreach (var row in this)
            {
                var additionalValues = additionalData.Length > rowIndex ? additionalData[rowIndex] : Enumerable.Empty<string>();
                addRow(row.Values.Select(v => v.GetValueAsString()).Concat(additionalValues));
                rowIndex++;
            }

            var stringBuilder = new StringBuilder().AppendLine();
            foreach (var row in rows)
            {
                WriteExampleRow(row, maxWidth, stringBuilder);
            }

            return stringBuilder.ToString();
        }

        public override string ToString()
        {
            return ToString(new string[0], new string[0][]);
        }

        private void WriteExampleRow(string[] row, int[] maxWidth, StringBuilder stringBuilder)
        {
            for (var index = 0; index < row.Length; index++)
            {
                var col = row[index];
                stringBuilder.AppendFormat("| {0} ", (col ?? string.Empty).Trim().PadRight(maxWidth[index]));
            }

            stringBuilder.AppendLine("|");
        }
    }
}