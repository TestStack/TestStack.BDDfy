using NUnit.Framework;
using Shouldly;

namespace TestStack.BDDfy.Tests
{
    public class NetToStringTests
    {
        [Test]
        public void ConvertertsStepNameContainingExample()
        {
            NetToString.Convert("GivenSomethingWith__example__InTitle")
                .ShouldBe("Given something with <example> in title");

            NetToString.Convert("GivenMethodTaking__ExampleInt__")
                .ShouldBe("Given method taking <example int>");
        } 
    }
}