using System.Linq;

namespace TestStack.BDDfy
{
    public static class WithExamplesExtensions
    {
        public static ExampleTable WithExamples(this object testObject, ExampleTable table)
        {
            table.TestObject = testObject;
            return table;
        }

        public static ExampleTable WithExamples(this object testObject, string table)
        {
            return testObject.WithExamples(Parse(table));
        }

        static ExampleTable Parse(string table)
        {
            var lines = table.Trim().Split('\n').Select(l => l.Trim().Trim('|'));
            var firstRow = lines.First();
            var headers = firstRow.Split('|').Select(h => h.Trim().Replace(" ", string.Empty)).ToArray();

            var exampleTable = new ExampleTable(headers);
            foreach (var line in lines.Skip(1))
            {
                exampleTable.Add(GetValues(line));
            }

            return exampleTable;
        }

        static object[] GetValues(string row)
        {
            return row.Split('|').Select(h => (object)h.Trim()).ToArray();
        }
    }
}
