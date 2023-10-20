namespace ScooterRental.Exceptions
{
    public class NegativeScooterPriceException : Exception
    {
        public NegativeScooterPriceException() : base("Price cant be negative")
        {
        }
    }
}
