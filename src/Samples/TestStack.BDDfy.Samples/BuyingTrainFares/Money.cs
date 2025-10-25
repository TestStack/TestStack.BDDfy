using System.Globalization;

namespace TestStack.BDDfy.Samples.BuyingTrainFares
{
    class Money(decimal amount)
    {
        public decimal Amount { get; set; } = amount;

        protected bool Equals(Money other)
        {
            return Amount == other.Amount;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Money)obj);
        }

        public override int GetHashCode()
        {
            return Amount.GetHashCode();
        }

        public static bool operator ==(Money left, Money right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Money left, Money right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return Amount.ToString("C", new CultureInfo("EN-US"));
        }
    }
}