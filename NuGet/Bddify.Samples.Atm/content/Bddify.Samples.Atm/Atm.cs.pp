using System;

namespace $rootnamespace$.Bddify.Samples.Atm
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