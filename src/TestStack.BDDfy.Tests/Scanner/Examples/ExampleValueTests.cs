using System;
using Shouldly;
using Xunit;

namespace TestStack.BDDfy.Tests.Scanner.Examples
{
    public class ExampleValueTests
    {
        [Fact]
        public void CanFormatAsStringTests()
        {
            new ExampleValue("Header", null, () => 0).GetValueAsString().ShouldBe("<null>");
            new ExampleValue("Header", 1, () => 0).GetValueAsString().ShouldBe("1");
            new ExampleValue("Header", new object(), () => 0).GetValueAsString().ShouldBe("System.Object");
            new ExampleValue("Header", new[] {1, 2}, () => 0).GetValueAsString().ShouldBe("1, 2");
        }

        [Fact]
        public void GetValue_WhenTargetTypeMatchesDirectly_ReturnsValue()
        {
            var value = new ExampleValue("Col", 42, () => 0);
            value.GetValue(typeof(int)).ShouldBe(42);
            value.ValueHasBeenUsed.ShouldBeTrue();
        }

        [Fact]
        public void GetValue_WhenNullAndTargetIsNullable_ReturnsNull()
        {
            var value = new ExampleValue("Col", null, () => 0);
            value.GetValue(typeof(int?)).ShouldBeNull();
            value.ValueHasBeenUsed.ShouldBeTrue();
        }

        [Fact]
        public void GetValue_WhenNullAndTargetIsReferenceType_ReturnsNull()
        {
            var value = new ExampleValue("Col", null, () => 0);
            value.GetValue(typeof(string)).ShouldBeNull();
        }

        [Fact]
        public void GetValue_WhenNullAndTargetIsValueType_Throws()
        {
            var value = new ExampleValue("Col", null, () => 2);
            var ex = Should.Throw<ArgumentException>(() => value.GetValue(typeof(int)));
            ex.Message.ShouldContain("Cannot convert <null> to Int32");
            ex.Message.ShouldContain("Column: 'Col'");
            ex.Message.ShouldContain("Row: 3");
        }

        [Theory]
        [InlineData("123", typeof(int), 123)]
        [InlineData("45.6", typeof(double), 45.6)]
        [InlineData("true", typeof(bool), true)]
        public void GetValue_UsesConvertChangeType(string input, Type targetType, object expected)
        {
            var value = new ExampleValue("Col", input, () => 0);
            value.GetValue(targetType).ShouldBe(expected);
        }

        [Fact]
        public void GetValue_WhenEnumString_ParsesEnum()
        {
            var value = new ExampleValue("Col", "Transition", () => 0);
            value.GetValue(typeof(ExecutionOrder)).ShouldBe(ExecutionOrder.Transition);
        }

        [Fact]
        public void GetValue_WhenDateTimeString_ParsesDateTime()
        {
            var value = new ExampleValue("Col", "2023-06-15", () => 0);
            value.GetValue(typeof(DateTime)).ShouldBe(new DateTime(2023, 6, 15));
        }

        [Fact]
        public void GetValue_WhenInvalidCast_ThrowsUnassignableExampleException()
        {
            var value = new ExampleValue("Col", new object(), () => 1);
            var ex = Should.Throw<UnassignableExampleException>(() => value.GetValue(typeof(int)));
            ex.Message.ShouldContain("cannot be assigned to Int32");
            ex.Message.ShouldContain("Column: 'Col'");
            ex.Message.ShouldContain("Row: 2");
        }

        [Theory]
        [InlineData("myHeader", "myHeader", true)]
        [InlineData("my header", "myHeader", true)]
        [InlineData("my_header", "myHeader", true)]
        [InlineData("MyHeader", "myheader", true)]
        [InlineData("Header1", "Header2", false)]
        [InlineData("Header", null, false)]
        public void MatchesName_VariousInputs(string header, string? name, bool expected)
        {
            var value = new ExampleValue(header, "x", () => 0);
            value.MatchesName(name).ShouldBe(expected);
        }

        [Fact]
        public void Row_ReturnsOneBasedIndex()
        {
            var value = new ExampleValue("Col", "x", () => 4);
            value.Row.ShouldBe(5);
        }
    }
}