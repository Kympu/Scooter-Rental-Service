namespace ScooterRental
{
    public class Accounting
    {
        public decimal CalculateCost(DateTime rentStart, DateTime rentEnd, decimal rate, decimal maxPrice)
        {
            double rentalDuration = (rentEnd - rentStart).TotalMinutes;
            decimal rentalPrice;

            if (rentEnd.Day - rentStart.Day > 0)
            {
                rentalPrice = CalculateMultipleDays(rentStart, rentEnd, rate, rentalDuration, maxPrice);
            }
            else
            {
                rentalPrice = CalculateSingleDay(rentStart, rentEnd, rate, rentalDuration, maxPrice);
            }

            return rentalPrice;
        }

        private decimal CalculateMultipleDays(DateTime rentStart, DateTime rentEnd, decimal rate, double rentalDuration, decimal maxPrice)
        {
            decimal rentalPrice = 0;

            int firstDayMinutes = 1440 - (rentStart.Hour * 60 + rentStart.Minute);
            int lastDayMinutes = rentEnd.Hour * 60 + rentEnd.Minute;
            int restOfMinutes = (int)(rentalDuration - firstDayMinutes - lastDayMinutes);

            rentalPrice += Math.Min((firstDayMinutes * rate), maxPrice);
            rentalPrice += Math.Min((lastDayMinutes * rate), maxPrice);
            rentalPrice += Math.Min(restOfMinutes * rate, maxPrice) * (restOfMinutes / 1440);

            return rentalPrice;
        }

        private decimal CalculateSingleDay(DateTime rentStart, DateTime rentEnd, decimal rate, double rentalDuration, decimal maxPrice)
        {
            int minutes = (rentEnd.Hour * 60 + rentEnd.Minute)
                    - (rentStart.Hour * 60 + rentStart.Minute);

            return Math.Min((minutes * rate), maxPrice);
        }
    }
}
