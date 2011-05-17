using Bddify.Core;
using Bddify.Scanners.GwtAttributes;

namespace Bddify.Samples.TicTacToe
{
    public class WhenXAndOPlayTheirFirstMoves : NewGame
    {
        [When]
        void AfterTwoMoves()
        {
            Game.PlayAt(0, 0);
            Game.PlayAt(2, 2);
        }

        [Then]
        [RunStepWithArgs(
                new[] {X, N, N},
                new[] {N, N, N},
                new[] {N, N, O},
                StepTextTemplate = "Then the board should look like [{0}], [{1}] and [{2}]")]
        void TheBoardStateShouldBe(string[] firstRow, string[] secondRow, string[] thirdRow)
        {
            Game.VerifyBoardState(firstRow, secondRow, thirdRow);
        }
    }
}