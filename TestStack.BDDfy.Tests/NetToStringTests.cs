using NUnit.Framework;
using Shouldly;
using TestStack.BDDfy.Configuration;

namespace TestStack.BDDfy.Tests
{
    [TestFixture]
    public class NetToStringTests
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
            Assert.That(Configurator.Scanners.Humanize("XIsFirstPlayer"), Is.EqualTo("X is first player"));
        }

        [TestCase("GivenThereAre__start__Cucumbers", "Given there are <start> cucumbers")]
        [TestCase("Given_there_are__start__cucumbers", "Given there are <start> cucumbers")]
        [TestCase("GivenMethodTaking__ExampleInt__", "Given method taking <example int>")]
        [TestCase("Given_method_taking__ExampleInt__", "Given method taking <ExampleInt>")]
        [TestCase("__starting__with_example", "<starting> with example")]
        [TestCase("__starting__WithExample", "<starting> with example")]
        [TestCase("WhenMethod__takes____two__parameters", "When method <takes> <two> parameters")]
        [TestCase("When_method__takes____two__parameters", "When method <takes> <two> parameters")]
        [TestCase("When_method_takes__one__and__two__parameters", "When method takes <one> and <two> parameters")]
        [TestCase("WhenMethodTakes__one__and__two__parameters", "When method takes <one> and <two> parameters")]
        public void CanDealWithExampleStepNames(string stepName, string expectedStepTitle)
        {
            NetToString.Convert(stepName).ShouldBe(expectedStepTitle, Case.Sensitive);
        }
    }
}