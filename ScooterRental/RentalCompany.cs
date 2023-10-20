using ScooterRental.Exceptions;

namespace ScooterRental
{
    public class RentalCompany : IRentalCompany
    {
        private readonly IScooterService _scooterService;
        private readonly List<RentedScooter> _rentedScooterList;
        private readonly Accounting _accounting = new Accounting();

        public RentalCompany(string name, IScooterService scooterService, List<RentedScooter> rentedScooterList)
        {
            Name = name;
            _scooterService = scooterService;
            _rentedScooterList = rentedScooterList;
        }

        public string Name { get; }

        public void StartRent(string id)
        {
            var scooter = _scooterService.GetScooterById(id);

            if (scooter.IsRented)
            { 
                throw new ScooterAlreadyRentedException(); 
            }

            scooter.IsRented = true;
            _rentedScooterList.Add(new RentedScooter(scooter.Id, DateTime.Now));
        }

        public decimal EndRent(string id)
        {
            var scooter = _scooterService.GetScooterById(id);
            var rentalRecord = _rentedScooterList.FirstOrDefault(s => 
                s.Id == scooter.Id && !s.TotalRentPrice.HasValue);

            decimal maxPrice = 20m;
            decimal rate = scooter.PricePerMinute;

            if (!rentalRecord.RentEnd.HasValue)
            {
                rentalRecord.RentEnd = DateTime.Now;
            }

            var rentalDuration = (rentalRecord.RentEnd - rentalRecord.RentStart).Value.TotalMinutes;

            decimal rentalPrice = _accounting.CalculateCost(rentalRecord.RentStart, 
                rentalRecord.RentEnd.Value, scooter.PricePerMinute, maxPrice);

            rentalRecord.TotalRentPrice = rentalPrice;
            scooter.IsRented = false;

            return rentalPrice;
        }

        public decimal CalculateIncome(int? year, bool includeNotCompletedRentals)
        {
            decimal totalProfits = 0;
            var rentedScooters = _rentedScooterList;

            if (rentedScooters.Count == 0)
            {
                throw new RentedScooterListIsEmptyException();
            }

            if (includeNotCompletedRentals)
            {
                rentedScooters
                    .Where(s => !s.RentEnd.HasValue)
                    .ToList()
                    .ForEach(s =>
                    { 
                        s.RentEnd = DateTime.Now;
                        EndRent(s.Id);
                    });
            }

            if (year.HasValue)
            {
                if (!rentedScooters.Any(scooter => scooter.RentEnd.HasValue && scooter.RentEnd.Value.Year == year))
                {
                    throw new NoScootersFoundInYearException(year.Value);
                }

                totalProfits = rentedScooters
                        .Where(scooter => scooter.RentEnd.HasValue && scooter.RentEnd.Value.Year == year)
                        .Sum(scooter => scooter.TotalRentPrice.Value);
            }
            else
            {
                totalProfits = rentedScooters.Where(s => s.TotalRentPrice.HasValue).ToList().Sum(s => s.TotalRentPrice.Value);
            }

            return totalProfits;
        }
    }
}
 