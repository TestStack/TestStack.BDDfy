using System.Collections.Generic;
using Shouldly;

namespace TestStack.BDDfy.Samples.TicTacToe
{
    public class WinnerGame(string[] firstRow, string[] secondRow, string[] thirdRow, string expectedWinner): GameUnderTest
    {
        private readonly string[] _firstRow = firstRow;
        private readonly string[] _secondRow = secondRow;
        private readonly string[] _thirdRow = thirdRow;
        private readonly string _expectedWinner = expectedWinner;

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
            Game.Winner.ShouldBe(_expectedWinner);
        }
    }
}