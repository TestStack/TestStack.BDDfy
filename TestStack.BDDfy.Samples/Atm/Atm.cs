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

namespace TestStack.BDDfy.Samples.Atm
{
    public class Atm
    {
        public int ExistingCash { get; private set; }

        public Atm(int existingCash)
        {
            ExistingCash = existingCash;
        }

        public void RequestMoney(Card card, int request)
        {
            if (!card.Enabled)
            {
                CardIsRetained = true;
                Message = DisplayMessage.CardIsRetained;
                return;
            }

            if(card.AccountBalance < request)
            {
                Message = DisplayMessage.InsufficientFunds;
                return;
            }

            DispenseValue = request;
            card.AccountBalance -= request;
        }

        public int DispenseValue { get; set; }

        public bool CardIsRetained { get; private set; }

        public DisplayMessage Message { get; private set; }
    }

    public class Card
    {
        public int AccountBalance { get; set; }
        private readonly bool _enabled;

        public Card(bool enabled, int accountBalance)
        {
            AccountBalance = accountBalance;
            _enabled = enabled;
        }

        public bool Enabled
        {
            get { return _enabled; }
        }
    }

    public enum DisplayMessage
    {
        None = 0,
        CardIsRetained,
        InsufficientFunds
    }
}