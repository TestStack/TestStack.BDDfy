using System;
using NUnit.Framework;
using Shouldly;

namespace TestStack.BDDfy.Tests.Scanner.StepScanners.Examples
{
    public class ExampleValueTests
    {
        [Test]
        public void CanFormatAsStringTests()
        {
            new ExampleValue("Header", null, () => 0).GetValueAsString().ShouldBe("'null'");
            new ExampleValue("Header", 1, () => 0).GetValueAsString().ShouldBe("1");
            new ExampleValue("Header", new Object(), () => 0).GetValueAsString().ShouldBe("System.Object");
            new ExampleValue("Header", new[] {1, 2}, () => 0).GetValueAsString().ShouldBe("1, 2");
        }
    }
}