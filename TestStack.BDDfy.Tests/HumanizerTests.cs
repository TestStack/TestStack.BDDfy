using NUnit.Framework;
using Shouldly;
using TestStack.BDDfy.Configuration;

namespace TestStack.BDDfy.Tests
{
    [TestFixture]
    public class HumanizerTests
    {
        [Test]
        public void PascalCaseInputStringIsTurnedIntoSentence()
        {
            Assert.That(
                Configurator.Scanners.Humanize("PascalCaseInputStringIsTurnedIntoSentence"), 
                Is.EqualTo("Pascal case input string is turned into sentence"));
        }

        [Test]
        public void WhenInputStringContainsConsequtiveCaptialLetters_ThenTheyAreTurnedIntoOneLetterWords()
        {
            Assert.That(Configurator.Scanners.Humanize("WhenIUseAnInputAHere"), Is.EqualTo("When I use an input a here"));
        }

        [Test]
        public void WhenInputStringStartsWithANumber_ThenNumberIsDealtWithLikeAWord()
        {
            Assert.That(Configurator.Scanners.Humanize("10NumberIsInTheBegining"), Is.EqualTo("10 number is in the begining"));
        }

        [Test]
        public void WhenInputStringEndWithANumber_ThenNumberIsDealtWithLikeAWord()
        {
            Assert.That(Configurator.Scanners.Humanize("NumberIsAtTheEnd100"), Is.EqualTo("Number is at the end 100"));
        }

        [Test]
        public void UnderscoredInputStringIsTurnedIntoSentence()
        {
            Assert.That(
                Configurator.Scanners.Humanize("Underscored_input_string_is_turned_into_sentence"), 
                Is.EqualTo("Underscored input string is turned into sentence"));
        }

        [Test]
        public void UnderscoredInputStringPreservesCasing()
        {
            Assert.That(
                Configurator.Scanners.Humanize("Underscored_input_String_is_turned_INTO_sentence"), 
                Is.EqualTo("Underscored input String is turned INTO sentence"));
        }

        [Test]
        public void OneLetterWordInTheBeginningOfStringIsTurnedIntoAWord()
        {
            Assert.That(
                Configurator.Scanners.Humanize("XIsFirstPlayer"),
                Is.EqualTo("X is first player"));
        }

        [Test]
        public void CanDealWithExampleStepNames()
        {
            NetToString.Convert("GivenThereAre__start__Cucumbers").ShouldBe("Given there are <start> cucumbers");
            NetToString.Convert("Given_there_are__start__cucumbers").ShouldBe("Given there are <start> cucumbers");
            NetToString.Convert("GivenMethodTaking__ExampleInt__").ShouldBe("Given method taking <example int>");
        }
    }
}