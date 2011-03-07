namespace SystemUnderTest
{
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
}