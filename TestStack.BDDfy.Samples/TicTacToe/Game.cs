// Copyright (C) 2011, Mehdi Khalili
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//     * Redistributions of source code must retain the above copyright
//       notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright
//       notice, this list of conditions and the following disclaimer in the
//       documentation and/or other materials provided with the distribution.
//     * Neither the name of the <organization> nor the
//       names of its contributors may be used to endorse or promote products
//       derived from this software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
// DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System.Collections.Generic;
using System.Linq;

namespace TestStack.BDDfy.Samples.TicTacToe
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