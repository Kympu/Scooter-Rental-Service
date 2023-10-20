namespace ScooterRental.Exceptions
{
    public class ScooterNotRentedException : Exception
    {
        public ScooterNotRentedException() : base("Scooter is not rented")
        {
        }
    }
}
