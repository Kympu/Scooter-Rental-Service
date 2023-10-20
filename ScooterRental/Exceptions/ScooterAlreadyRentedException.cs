namespace ScooterRental.Exceptions
{
    public class ScooterAlreadyRentedException : Exception
    {
        public ScooterAlreadyRentedException() : base("Scooter is already rented")
        {
        }
    }
}
