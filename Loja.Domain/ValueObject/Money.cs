namespace Loja.Domain.ValueObject
{
    public class Money
    {
        public decimal Value { get; set; }
        public string Currency { get; set; }

        public Money(decimal value, string currency = "BRL")
        {
            Value = value;
            Currency = currency;
        }

        public static Money operator *(Money money, Money multiplier)
        {
            if (money.Currency != multiplier.Currency)
                throw new InvalidOperationException("Cannot add money values with different currencies");

            return new Money(money.Value * multiplier.Value, money.Currency);
        }

        public static Money operator -(Money a, Money b)
        {
            if (a.Currency != b.Currency)
                throw new InvalidOperationException("Cannot subtract money values with different currencies");

            return new Money(a.Value - b.Value, a.Currency);
        }

        public static Money operator *(Money a, decimal multiplier)
        {
            return new Money(a.Value * multiplier, a.Currency);
        }

        public static bool operator ==(Money a, Money b) => a.Equals(b);
        public static bool operator !=(Money a, Money b) => !a.Equals(b);
        public static bool operator >(Money a, Money b) => a.CompareTo(b) > 0;
        public static bool operator <(Money a, Money b) => a.CompareTo(b) < 0;
        public static bool operator >=(Money a, Money b) => a.CompareTo(b) >= 0;
        public static bool operator <=(Money a, Money b) => a.CompareTo(b) <= 0;

        public override bool Equals(object obj)
        {
            return obj is Money money && Equals(money);
        }

        public bool Equals(Money other)
        {
            return Value == other.Value && Currency == other.Currency;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value, Currency);
        }

        public int CompareTo(Money other)
        {
            if (Currency != other.Currency)
                throw new InvalidOperationException("Cannot compare money values with different currencies");

            return Value.CompareTo(other.Value);
        }

        public override string ToString()
        {
            return $"{Value:F2} {Currency}";
        }
    }
}
