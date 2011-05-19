using System.Collections.Generic;
using System.Linq;

namespace Bddify.Samples.TicTacToe
{
    public class Game
    {
        public const string X = "X";
        public const string O = "O";
        public const string N = " ";

        private static readonly string[] EmptyRow = Enumerable.Repeat(N, 3).ToArray();
        readonly string[][] _board;

        public Game()
            : this(EmptyRow, EmptyRow, EmptyRow)
        {
        }

        public Game(string[] firstRow, string[] secondRow, string[] thirdRow)
        {
            _board = new string[3][]; 
            _board[0] = (string[])firstRow.Clone();
            _board[1] = (string[])secondRow.Clone();
            _board[2] = (string[])thirdRow.Clone();
        }

        public string Winner
        {
            get 
            {
                var winnerLine = Lines.FirstOrDefault(line => line.All(l => l == X) || line.All(l => l == O));
                if (winnerLine == null)
                    return null;

                return winnerLine.First();
            }
        }

        IEnumerable<string[]> Lines
        {
            get
            {
                for (int i = 0; i < 3; i++)
                {
                    yield return new[] { _board[i][0], _board[i][1], _board[i][2] };
                    yield return new[] { _board[0][i], _board[1][i], _board[2][i] };
                }

                yield return new[] { _board[0][0], _board[1][1], _board[2][2] };
                yield return new[] { _board[2][0], _board[1][1], _board[0][2] };
            }
        }

        public void PlayAt(int row, int column)
        {
            var emptyCellsCount = _board.SelectMany(s=>s).Count(s => s == N);
            var player = X;
            if (emptyCellsCount % 2 == 0)
                player = O;

            _board[row][column] = player;
        }

        public override bool Equals(object obj)
        {
            var game = obj as Game;
            return game != null && base.Equals(game);
        }

        public bool Equals(Game other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            for (int i = 0; i < 3; i++)
            {
                if(!_board[i].SequenceEqual(other._board[i]))
                    return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return (_board != null ? _board.GetHashCode() : 0);
        }
    }
}