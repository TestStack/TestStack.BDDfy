namespace TestStack.BDDfy.Samples.Atm
{
    public class Atm(int existingCash)
    {
        public int ExistingCash { get; private set; } = existingCash;

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

    public class Card(bool enabled, int accountBalance)
    {
        public int AccountBalance { get; set; } = accountBalance;
        private readonly bool _enabled = enabled;

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