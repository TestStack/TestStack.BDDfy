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