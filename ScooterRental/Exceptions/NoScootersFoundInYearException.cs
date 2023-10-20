namespace ScooterRental.Exceptions
{
    public class NoScootersFoundInYearException : Exception
    {
        public NoScootersFoundInYearException(int year) : base($"No Scooters found in {year}")
        {
        }
    }
}
