using System.Collections.Generic;
using NUnit.Framework;

namespace Bddify.Samples.TicTacToe
{
    public class WinnerGame : GameUnderTest
    {
        private readonly string[] _firstRow;
        private readonly string[] _secondRow;
        private readonly string[] _thirdRow;
        private readonly string _expectedWinner;

        public WinnerGame(string[] firstRow, string[] secondRow, string[] thirdRow, string expectedWinner)
        {
            _firstRow = firstRow;
            _secondRow = secondRow;
            _thirdRow = thirdRow;
            _expectedWinner = expectedWinner;
        }

        // Note: This method returns IEnumerable<string>
        // this is done to allow the method to return its title.
        // if you use this method make sure to yield return the title as the very first action
        IEnumerable<string> GivenTheFollowingBoard()
        {
            yield return string.Format(
                BoardStateTemplate, 
                string.Join(", ", _firstRow), 
                string.Join(", ", _secondRow), 
                string.Join(", ", _thirdRow));

            Game = new Game(_firstRow, _secondRow, _thirdRow);
        }

        // Note: This method returns IEnumerable<string>
        // this is done to allow the method to return its title.
        // if you use this method make sure to yield return the title as the very first action
        IEnumerable<string> ThenTheWinnerShouldBe()
        {
            yield return "Then the winner is " + _expectedWinner;
            Assert.AreEqual(Game.Winner, _expectedWinner);
        }
    }
}