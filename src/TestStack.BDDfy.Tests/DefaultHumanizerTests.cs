using System;
using Shouldly;
using TestStack.BDDfy.Configuration;
using Xunit;

namespace TestStack.BDDfy.Tests
{
    public class DefaultHumanizerTests
    {
        private static DefaultHumanizer Humanizer => new();

        [Fact]
        public void PascalCaseInputStringIsTurnedIntoSentence()
        {
            Humanizer.Humanize("PascalCaseInputStringIsTurnedIntoSentence")
                .ShouldBe("Pascal case input string is turned into sentence");
        }

        [Fact]
        public void WhenInputStringContainsConsequtiveCaptialLetters_ThenTheyAreTurnedIntoOneLetterWords()
        {
            Humanizer.Humanize("WhenIUseAnInputAHere").ShouldBe("When I use an input a here");
        }

        [Fact]
        public void WhenInputStringStartsWithANumber_ThenNumberIsDealtWithLikeAWord()
        {
            Humanizer.Humanize("10NumberIsInTheBegining").ShouldBe("10 number is in the begining");
        }

        [Fact]
        public void WhenInputStringEndWithANumber_ThenNumberIsDealtWithLikeAWord()
        {
            Humanizer.Humanize("NumberIsAtTheEnd100").ShouldBe("Number is at the end 100");
        }

        [Fact]
        public void UnderscoredInputStringIsTurnedIntoSentence()
        {
            Humanizer.Humanize("Underscored_input_string_is_turned_into_sentence")
                .ShouldBe("Underscored input string is turned into sentence");
        }

        [Fact]
        public void UnderscoredInputStringPreservesCasing()
        {
            Humanizer.Humanize("Underscored_input_String_is_turned_INTO_sentence")
                .ShouldBe("Underscored input String is turned INTO sentence");
        }

        [Fact]
        public void OneLetterWordInTheBeginningOfStringIsTurnedIntoAWord()
        {
            Humanizer.Humanize("XIsFirstPlayer").ShouldBe("X is first player");
        }

        [Theory]
        [InlineData("GivenThereAre__start__Cucumbers", "Given there are <start> cucumbers")]
        [InlineData("Given_there_are__start__cucumbers", "Given there are <start> cucumbers")]
        [InlineData("GivenThereAre__count1__Cucumbers", "Given there are <count 1> cucumbers")]
        [InlineData("Given_there_are__count2__cucumbers", "Given there are <count2> cucumbers")] // The spacing rules for numbers are not consequential
        [InlineData("GivenMethodTaking__ExampleInt__", "Given method taking <example int>")]
        [InlineData("Given_method_taking__ExampleInt__", "Given method taking <ExampleInt>")]
        [InlineData("__starting__with_example", "<starting> with example")]
        [InlineData("__starting__WithExample", "<starting> with example")]
        [InlineData("WhenMethod__takes____two__parameters", "When method <takes> <two> parameters")]
        [InlineData("When_method__takes____two__parameters", "When method <takes> <two> parameters")]
        [InlineData("When_method_takes__one__and__two__parameters", "When method takes <one> and <two> parameters")]
        [InlineData("WhenMethodTakes__one__and__two__parameters", "When method takes <one> and <two> parameters")]
        public void CanDealWithExampleStepNames(string stepName, string expectedStepTitle)
        {
            Humanizer.Humanize(stepName).ShouldBe(expectedStepTitle);
        }

        [Theory]
        [InlineData("GivenThereAre__tv�__Cucumbers", "Given there are <tv�> cucumbers")]
        public void ReportsIllegalExampleStepNames(string stepName, string expectedStepTitle) {
            var exception = Record.Exception(() => {
                Humanizer.Humanize(stepName).ShouldBe(expectedStepTitle);
            });

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<ArgumentException>();
        }
    }
}