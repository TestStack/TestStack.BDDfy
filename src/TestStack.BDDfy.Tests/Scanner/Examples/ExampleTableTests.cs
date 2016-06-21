#if Approvals
using System;
using System.Linq;
using Shouldly;
using Xunit;

namespace TestStack.BDDfy.Tests.Scanner.Examples
{
    public class ExampleTableTests
    {
        [Fact]
        public void CanParseTable()
        {
            const string table = @"
| Header 1 | Header 2     | Header3   |
| Value 1  | 2            | 3          |
|          | 14 Mar 2010  | Transition |";

            var exampleTable = ExampleTable.Parse(table);

            exampleTable.Headers.ShouldBe(new[] { "Header 1", "Header 2", "Header3" });
            exampleTable.ElementAt(0).GetValueOf(0, typeof(string)).ShouldBe("Value 1");
            exampleTable.ElementAt(0).GetValueOf(1, typeof(int)).ShouldBe(2);
            exampleTable.ElementAt(0).GetValueOf(2, typeof(decimal)).ShouldBe(3m);
            exampleTable.ElementAt(1).GetValueOf(0, typeof(string)).ShouldBe(null);
            exampleTable.ElementAt(1).GetValueOf(0, typeof(int?)).ShouldBe(null);
            exampleTable.ElementAt(1).GetValueOf(2, typeof(ExecutionOrder)).ShouldBe(ExecutionOrder.Transition);
            var argException = Should.Throw<ArgumentException>(() => exampleTable.ElementAt(1).GetValueOf(0, typeof(int)));
            argException.Message.ShouldBe("Cannot convert <null> to Int32 (Column: 'Header 1', Row: 2)");
            exampleTable.ElementAt(1).GetValueOf(1, typeof(DateTime)).ShouldBe(new DateTime(2010, 3, 14));
        }

        [Fact]
        public void TableToString()
        {
            var table = new ExampleTable("Header 1", "Header 2")
            {
                {1, 2},
                {3, 4}
            };

            table.ToString().ShouldMatchApproved();
        }

        [Fact]
        public void TableToStringWithAdditionalColumn()
        {
            var table = new ExampleTable("Header 1", "Header 2")
            {
                {1, 2},
                {3, 4}
            };

            table.ToString(new[] {"Additional"}, new[] {new[] {"SomeAdditional Value"}})
                .ShouldMatchApproved();
        }
    }
}
#endif