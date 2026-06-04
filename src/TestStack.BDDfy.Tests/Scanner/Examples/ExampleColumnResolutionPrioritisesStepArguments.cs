using Shouldly;
using Xunit;

namespace TestStack.BDDfy.Tests.Scanner.Examples
{
    /// <summary>
    /// Regression test for https://github.com/TestStack/TestStack.BDDfy/issues/235
    /// Step arguments should take priority over fields/properties when resolving
    /// example column values by name.
    /// </summary>
    public class ExampleColumnResolutionPrioritisesStepArguments
    {
        public enum MyEnumType
        {
            VeryVeryVeryVeryVerySuperLongName
        }

        private readonly MyEnumType _sname = MyEnumType.VeryVeryVeryVeryVerySuperLongName;

        private void GivenSomething() { }
        private void WhenSomethingElse(string sname) => sname.ShouldBe("hello");
        private void ThenResult() { }

        [Fact]
        public void ShouldSetStepArgumentInsteadOfIncompatibleField()
        {
            var sname = default(string);

            this.Given(_ => _.GivenSomething())
                .When(_ => _.WhenSomethingElse(sname!))
                .Then(_ => _.ThenResult())
                .WithExamples(
                    new ExampleTable("sname")
                    {
                        { "hello" },
                    })
                .BDDfy();

            _sname.ShouldBe(MyEnumType.VeryVeryVeryVeryVerySuperLongName);
        }
    }
}
