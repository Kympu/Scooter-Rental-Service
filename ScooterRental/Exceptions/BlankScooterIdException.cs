namespace ScooterRental.Exceptions
{
    public class BlankScooterIdException : Exception
    {
        public BlankScooterIdException() : base("ID Cant be blank")
        {
        }
    }
}
