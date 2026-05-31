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

            table.ToString(new[] { "Additional" }, new[] { new[] { "SomeAdditional Value" } })
                .ShouldMatchApproved();
        }

        [Fact]
        public void Add_WhenColumnCountMismatch_Throws()
        {
            var table = new ExampleTable("A", "B");
            var ex = Should.Throw<ArgumentException>(() => table.Add(1, 2, 3));
            ex.Message.ShouldContain("Number of column values does not match");
        }

        [Fact]
        public void CollectionOperations_WorkCorrectly()
        {
            var table = new ExampleTable("A");
            table.Add("val1");
            table.Add("val2");

            table.Count.ShouldBe(2);
            table.IsReadOnly.ShouldBeFalse();

            var first = table.First();
            table.Contains(first).ShouldBeTrue();
            table.Remove(first).ShouldBeTrue();
            table.Count.ShouldBe(1);

            table.Clear();
            table.Count.ShouldBe(0);
        }

        [Fact]
        public void CopyTo_CopiesElements()
        {
            var table = new ExampleTable("A");
            table.Add("x");
            table.Add("y");

            var array = new Example[3];
            table.CopyTo(array, 1);

            array[0].ShouldBeNull();
            array[1].ShouldNotBeNull();
            array[2].ShouldNotBeNull();
        }

        [Theory]
        [InlineData("MyHeader", "my header", true)]
        [InlineData("MyHeader", "my_header", true)]
        [InlineData("MyHeader", "MYHEADER", true)]
        [InlineData("MyHeader", "Other", false)]
        [InlineData("MyHeader", null, false)]
        public void HeaderMatches_VariousCases(string header, string? name, bool expected)
        {
            ExampleTable.HeaderMatches(header, name).ShouldBe(expected);
        }
    }
}