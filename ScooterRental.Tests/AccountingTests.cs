
using FluentAssertions;
using ScooterRental.Exceptions;

namespace ScooterRental.Tests
{
    [TestClass]   
    public class AccountingTests
    {
        private IRentalCompany _rentalCompany;
        private const string DEFAULT_COMPANY_NAME = "default";
        private List<Scooter> _scooterList;
        private List<RentedScooter> _rentedScooters;

        [TestInitialize]
        public void Initialize()
        {
            _scooterList = new List<Scooter>
            {
                new Scooter("1", 0.2m),
                new Scooter("2", 0.2m),
                new Scooter("3", 0.1m)
            };

            _rentedScooters = new List<RentedScooter>()
            {
                new RentedScooter("1", new DateTime(2021, 9, 5, 23, 0, 0)) { RentEnd = new DateTime(2021, 9, 6, 0, 36, 0) }, // 19.2
                new RentedScooter("2", new DateTime(2021, 9, 5, 23, 0, 0)) { RentEnd = new DateTime(2021, 9, 6, 1, 0, 0) },  // 24
                new RentedScooter("3", new DateTime(2023, 9, 5, 23, 0, 0)) { RentEnd = new DateTime(2023, 9, 6, 1, 30, 0) } // 15
            };

            _rentalCompany = new RentalCompany(
                DEFAULT_COMPANY_NAME,
                new ScooterService(_scooterList),
                _rentedScooters);

            _rentedScooters.ForEach(scooter => _rentalCompany.EndRent(scooter.Id));
        }

        [TestMethod]
        public void CalculateIncome_ScootersRentedin2021_CorrectProfit43_2()
        {
            var profit = _rentalCompany.CalculateIncome(2021, false);

            profit.Should().Be(43.2m);
        }

        [TestMethod]
        public void CalculateIncome_NoScootersFoundInYear_ThrowsNoScooterFoundThisYearException()
        {
            int year = 2020;

            Action action = () => _rentalCompany.CalculateIncome(year, false);

            action.Should().Throw<NoScootersFoundInYearException>();
        }

        [TestMethod]
        public void CalculateIncome_NoScootersFound_ThrowsScooterListIsEmtpyException()
        {
            _rentedScooters.Clear();

            Action action = () => _rentalCompany.CalculateIncome(null, true);

            action.Should().Throw<RentedScooterListIsEmptyException>();
        }

        [TestMethod]
        public void CalculateIncome_includeNotCompletedRentalsIsTrue_ValueOfAllScooters()
        {
            _rentedScooters.Clear();
            _scooterList.Add(new Scooter("4", 0.1m));

            var rentedScooter4 = new RentedScooter("4", new DateTime(2023, 9, 9, 0, 0, 0));
            _rentedScooters.Add(rentedScooter4);

            var profit = _rentalCompany.CalculateIncome(null, true);

            profit.Should().BeGreaterThan(0);
        }

        [TestMethod]
        public void CalculateIncome_RentAScooterAgain_ProfitFromAllScooters()
        {
            var rentedScooter4 = new RentedScooter("1", 
                new DateTime(2021, 9, 5, 23, 0, 0)) { RentEnd = new DateTime(2021, 9, 6, 0, 36, 0) };

            _rentedScooters.Add(rentedScooter4);
            _rentalCompany.EndRent("1");

            var profit = _rentalCompany.CalculateIncome(null, true);

            profit.Should().Be(77.4m);
        }

    }
}
