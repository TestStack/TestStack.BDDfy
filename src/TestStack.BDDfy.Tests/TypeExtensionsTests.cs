using System;
using Shouldly;
using Xunit;

namespace TestStack.BDDfy.Tests
{
    public class TypeExtensionsTests
    {
        [Theory]
        [InlineData(typeof(int), true)]
        [InlineData(typeof(string), false)]
        [InlineData(typeof(DateTime), true)]
        [InlineData(typeof(object), false)]
        public void IsValueType_ReturnsExpected(Type type, bool expected)
        {
            type.IsValueType().ShouldBe(expected);
        }

        [Theory]
        [InlineData(typeof(DayOfWeek), true)]
        [InlineData(typeof(int), false)]
        [InlineData(typeof(string), false)]
        public void IsEnum_ReturnsExpected(Type type, bool expected)
        {
            type.IsEnum().ShouldBe(expected);
        }

        [Theory]
        [InlineData(typeof(int?), true)]
        [InlineData(typeof(System.Collections.Generic.List<int>), true)]
        [InlineData(typeof(int), false)]
        [InlineData(typeof(string), false)]
        public void IsGenericType_ReturnsExpected(Type type, bool expected)
        {
            type.IsGenericType().ShouldBe(expected);
        }

        [Fact]
        public void IsInstanceOfType_ReturnsCorrectly()
        {
            typeof(string).IsInstanceOfType("hello").ShouldBeTrue();
            typeof(int).IsInstanceOfType("hello").ShouldBeFalse();
            typeof(object).IsInstanceOfType(42).ShouldBeTrue();
        }

        [Fact]
        public void Assembly_ReturnsCorrectAssembly()
        {
            typeof(string).Assembly().ShouldBe(typeof(string).Assembly);
        }

        [Fact]
        public void GetCustomAttributes_ReturnsAttributes()
        {
            var attrs = typeof(FlagsAttribute).GetCustomAttributes(typeof(AttributeUsageAttribute), true);
            attrs.ShouldNotBeEmpty();
            attrs[0].ShouldBeOfType<AttributeUsageAttribute>();
        }
    }
}
