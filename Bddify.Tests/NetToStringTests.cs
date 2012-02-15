using Bddify.Core;
using NUnit.Framework;

namespace Bddify.Tests
{
    public class NetToStringTests
    {
        [Test]
        public void PascalCaseInputStringIsTurnedIntoSentence()
        {
            Assert.That(
                NetToString.Convert("PascalCaseInputStringIsTurnedIntoSentence"), 
                Is.EqualTo("Pascal case input string is turned into sentence"));
        }

        [Test]
        public void WhenInputStringContainsConsequtiveCaptialLetters_ThenTheyAreTurnedIntoOneLetterWords()
        {
            Assert.That(NetToString.Convert("WhenIUseAnInputAHere"), Is.EqualTo("When I use an input a here"));
        }

        [Test]
        public void WhenInputStringStartsWithANumber_ThenNumberIsDealtWithLikeAWord()
        {
            Assert.That(NetToString.Convert("10NumberIsInTheBegining"), Is.EqualTo("10 number is in the begining"));
        }

        [Test]
        public void WhenInputStringEndWithANumber_ThenNumberIsDealtWithLikeAWord()
        {
            Assert.That(NetToString.Convert("NumberIsAtTheEnd100"), Is.EqualTo("Number is at the end 100"));
        }

        [Test]
        public void UnderscoredInputStringIsTurnedIntoSentence()
        {
            Assert.That(
                NetToString.Convert("Underscored_input_string_is_turned_into_sentence"), 
                Is.EqualTo("Underscored input string is turned into sentence"));
        }

        [Test]
        public void UnderscoredInputStringPreservesCasing()
        {
            Assert.That(
                NetToString.Convert("Underscored_input_String_is_turned_INTO_sentence"), 
                Is.EqualTo("Underscored input String is turned INTO sentence"));
        }

        [Test]
        public void OnLetterWordInTheBeginningOfStringIsTurnedIntoAWord()
        {
            Assert.That(
                NetToString.Convert("XIsFirstPlayer"),
                Is.EqualTo("X is first player"));
        }
    }
}