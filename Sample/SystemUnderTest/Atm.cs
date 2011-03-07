namespace SystemUnderTest
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
}