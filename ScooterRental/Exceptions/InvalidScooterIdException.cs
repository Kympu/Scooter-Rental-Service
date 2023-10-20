namespace ScooterRental.Exceptions
{
    public class InvalidScooterIdException : Exception
    {
        public InvalidScooterIdException() : base("No scooter with Id found")
        {
        }
    }
}
