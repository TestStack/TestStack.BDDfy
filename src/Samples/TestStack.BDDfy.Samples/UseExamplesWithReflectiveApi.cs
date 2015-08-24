using Shouldly;
using Xunit;

namespace TestStack.BDDfy.Samples
{
    public class UseExamplesWithReflectiveApi
    {
        private int _start;
        private int _eat;

        [Fact]
        public void CanRunExamplesWithReflectiveApi()
        {
            this.WithExamples(new ExampleTable("Start", "Eat", "Left")
                {
                    {12, 5, 7},
                    {20, 5, 15}
                })
                .BDDfy();
        }

        void GivenThereAre__start__Cucumbers(int start)
        {
            // the start argument is provided by the framework based on the example entries
            // please note that `start` argument name matches the `start` header from the examples
            // and also it has to match with the <start> placeholder in the step title which is created based on conventions
            _start = start;
        }

        [AndGiven("And I eat <eat> of them")]
        void WhenIEatAFewCucumbers(int eat)
        {
            // the eat argument is provided by the framework based on the example entries
            // please note that `eat` argument name matches the `start` header from the examples
            // and also it has to match the <eat> placeholder in the step title
            _eat = eat;
        }

        void ThenIShouldHave__left__Cucumbers(int left)
        {
            // like given and when steps left is provided here because it matches the example header and is found on the step title
            (_start - _eat).ShouldBe(left);
        }
    }
}
