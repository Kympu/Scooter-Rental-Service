namespace ScooterRental.Exceptions
{
    public class DuplicateScooterExcetpion : Exception
    {
        public DuplicateScooterExcetpion() : base("Scooter already exists")
        {
        }
    }
}
