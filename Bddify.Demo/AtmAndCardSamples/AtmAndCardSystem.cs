namespace Bddify.Demo.AtmAndCardSamples
{
    public class Atm
    {
        public void RequestMoney(Card card)
        {
            if (!card.Enabled)
            {
                CardIsRetained = true;
                Message = DisplayMessage.CardIsRetained;
            }
        }

        public bool CardIsRetained { get; private set; }

        public DisplayMessage Message { get; private set; }
    }

    public class Card
    {
        private readonly bool _enabled;

        public Card(bool enabled)
        {
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
        CardIsRetained
    }
}