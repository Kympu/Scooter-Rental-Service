namespace ScooterRental.Exceptions
{
    public class RentedScooterListIsEmptyException : Exception
    {
        public RentedScooterListIsEmptyException() : base("Rented scooter list is empty")
        {
        }
    }
}
