using FluentAssertions;
using ScooterRental.Exceptions;

namespace ScooterRental.Tests
{
    [TestClass]
    public class RentalCompanyTests
    {
        private IRentalCompany _rentalCompany;
        private IScooterService _scooterService;
        private const string DEFAULT_COMPANY_NAME = "default";
        private List<Scooter> _scooterList;
        private List<RentedScooter> _rentedScooters;
        private string DEFAULT_SCOOTER_ID = "101";

        [TestInitialize]
        public void Initialize()
        {
            _scooterList = new List<Scooter>();
            _rentedScooters = new List<RentedScooter>();
            _scooterService = new ScooterService(_scooterList);
            _rentalCompany = new RentalCompany(
                DEFAULT_COMPANY_NAME, 
                new ScooterService(_scooterList), 
                _rentedScooters);

            _scooterList.Add(new Scooter(DEFAULT_SCOOTER_ID, 0.2m));
        }

        [TestMethod]
        public void Name_GetCompanyName_SetToBeDefault()
        {
            _rentalCompany.Name.Should().Be(DEFAULT_COMPANY_NAME);
        }

        [TestMethod]
        public void StartRent_ScooterId_RentedIsTrue()
        {
            var scooter = _scooterService.GetScooterById(DEFAULT_SCOOTER_ID);

            _rentalCompany.StartRent(DEFAULT_SCOOTER_ID);

            scooter.IsRented.Should().BeTrue();
        }

        [TestMethod]
        public void StartRent_WrongScooterId_ThrowsInvalidScooterIdException()
        {
            var scooter = _scooterService.GetScooterById(DEFAULT_SCOOTER_ID);

            Action action = () => _rentalCompany.StartRent("A");

            action.Should().Throw<InvalidScooterIdException>();
        }

        [TestMethod]
        public void StartRent_RentARentedScooter_ThorwsScooterAlreadyRentedExcetpion()
        {
            _rentalCompany.StartRent(DEFAULT_SCOOTER_ID);
            Action action = () => _rentalCompany.StartRent(DEFAULT_SCOOTER_ID);

            action.Should().Throw<ScooterAlreadyRentedException>();
        }

        [TestMethod]
        public void EndRent_ScooterId_RentedIsFalse()
        {
            var scooter = _scooterService.GetScooterById(DEFAULT_SCOOTER_ID);

            _rentedScooters.Add(new RentedScooter(scooter.Id, DateTime.Now));
            _rentalCompany.EndRent(DEFAULT_SCOOTER_ID);

            scooter.IsRented.Should().BeFalse();
        }

        [TestMethod]
        public void EndRent_WrongScooterId_ThrowsInvalidScooterIdException()
        {
            var scooter = _scooterService.GetScooterById(DEFAULT_SCOOTER_ID);

            Action action = () => _rentalCompany.EndRent("2");

            action.Should().Throw<InvalidScooterIdException>();
        }

        [TestMethod]
        public void EndRent_RentFor2Hours_Returns20()
        {
            var rentedScooter = new RentedScooter(DEFAULT_SCOOTER_ID, new DateTime(2023, 9, 5, 21, 0, 0));
            rentedScooter.RentEnd = new DateTime(2023, 9, 5, 23, 0, 0);

            _rentedScooters.Add(rentedScooter);

            var price = _rentalCompany.EndRent(DEFAULT_SCOOTER_ID);

            price.Should().Be(20);
        }

        [TestMethod]
        public void EndRent_RentFor26HoursAnd12MinutesBetween3Days_Returns42point3()
        {
            var rentedScooter = new RentedScooter(DEFAULT_SCOOTER_ID, new DateTime(2023, 9, 5, 22, 0, 0));
            rentedScooter.RentEnd = new DateTime(2023, 9, 7, 0, 12, 0);

            _rentedScooters.Add(rentedScooter);

            var price = _rentalCompany.EndRent(DEFAULT_SCOOTER_ID);

            price.Should().Be(42.4m);
        }

        [TestMethod]
        public void EndRent_RentFor74HoursAnd36Minutes_Returns79point2()
        {
            var rentedScooter = new RentedScooter(DEFAULT_SCOOTER_ID, new DateTime(2023, 9, 5, 23, 0, 0));
            rentedScooter.RentEnd = new DateTime(2023, 9, 9, 0, 36, 0);

            _rentedScooters.Add(rentedScooter);

            var price = _rentalCompany.EndRent(DEFAULT_SCOOTER_ID);

            price.Should().Be(79.2m);
        }
    }
}
