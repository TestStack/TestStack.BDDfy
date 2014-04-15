using System;
using System.Linq;
using NUnit.Framework;
using Shouldly;

namespace TestStack.BDDfy.Tests.Scanner.StepScanners.Examples
{
    [TestFixture]
    public class ExampleTableTests
    {
        [Test]
        public void CanParseTable()
        {
            const string table = @"
| Header 1 | Header 2     | Header 3   |
| Value 1  | 2            | 3          |
|          | 14 Mar 2010  | Transition |";

            var exampleTable = this.WithExamples(table);

            exampleTable.Headers.ShouldBe(new[] { "Header1", "Header2", "Header3" });
            exampleTable.ElementAt(0).GetExampleValue(0, typeof(string)).ShouldBe("Value 1");
            exampleTable.ElementAt(0).GetExampleValue(1, typeof(int)).ShouldBe(2);
            exampleTable.ElementAt(0).GetExampleValue(2, typeof(decimal)).ShouldBe(3m);
            exampleTable.ElementAt(1).GetExampleValue(0, typeof(string)).ShouldBe(null);
            exampleTable.ElementAt(1).GetExampleValue(0, typeof(int?)).ShouldBe(null);
            exampleTable.ElementAt(1).GetExampleValue(2, typeof(ExecutionOrder)).ShouldBe(ExecutionOrder.Transition);
            var argException = Should.Throw<ArgumentException>(() => exampleTable.ElementAt(1).GetExampleValue(0, typeof(int)));
            argException.Message.ShouldBe(string.Format("Cannot convert {0} to {1}", "<null>", "Int32"));
            exampleTable.ElementAt(1).GetExampleValue(1, typeof(DateTime)).ShouldBe(new DateTime(2010, 3, 14));
        }
    }
}