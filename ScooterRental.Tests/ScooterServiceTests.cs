using FluentAssertions;
using ScooterRental.Exceptions;

namespace ScooterRental.Tests
{
    [TestClass]
    public class ScooterServiceTests
    {
        private ScooterService _scooterService;
        private List<Scooter> _scooterStorage;
        private const string DEFAULT_SCOOTER_ID = "1";
        private const decimal DEFAULT_PRICE_PER_MINUTE = 0.2m;

        [TestInitialize]
        public void Initialize()
        {
            _scooterStorage = new List<Scooter>();
            _scooterService = new ScooterService(_scooterStorage);
        }

        [TestMethod]
        public void AddScooter_WithIdAndPricePerMinute_ScooterAdded()
        {
            _scooterService.AddScooter(DEFAULT_SCOOTER_ID, DEFAULT_PRICE_PER_MINUTE);

            _scooterStorage.Count.Should().Be(1);
        }

        [TestMethod]
        public void AddScooter_WithId1AndPricePerMinute1_ScooterAddedWithId1andPricePerMinute1()
        {
            _scooterService.AddScooter(DEFAULT_SCOOTER_ID, 1m);

            var scooter = _scooterStorage.Last();

            scooter.Id.Should().Be("1");
            scooter.PricePerMinute.Should().Be(1m);
        }

        [TestMethod]
        public void AddScooter_AddDuplicateScooter_ThrowsDuplicateScooterExcetpion()
        {
            _scooterStorage.Add(new Scooter(DEFAULT_SCOOTER_ID, DEFAULT_PRICE_PER_MINUTE));

            Action action = () => _scooterService.AddScooter(DEFAULT_SCOOTER_ID, DEFAULT_PRICE_PER_MINUTE);
            
            action.Should().Throw<DuplicateScooterExcetpion>();
        }

        [TestMethod]
        public void AddScooter_AddScooterWithNegativePrice_ThrowsNegativeScooterPriceExcetpion()
        {
            Action action = () => _scooterService.AddScooter(DEFAULT_SCOOTER_ID, -1);

            action.Should().Throw<NegativeScooterPriceException>();
        }

        [TestMethod]
        public void AddScooter_AddScooterWithBlankId_ThrowsBlankScooterIdExcetpion()
        {
            Action action = () => _scooterService.AddScooter("", DEFAULT_PRICE_PER_MINUTE);

            action.Should().Throw<BlankScooterIdException>();
        }

        [TestMethod]
        public void GetScooterById_AddScooterWithId_ReturnScooterSameWithId()
        {
            _scooterService.AddScooter(DEFAULT_SCOOTER_ID, DEFAULT_PRICE_PER_MINUTE);
            Scooter getScooter = _scooterService.GetScooterById(DEFAULT_SCOOTER_ID);

            getScooter.Id.Should().Be(DEFAULT_SCOOTER_ID);
        }

        [TestMethod]
        public void GetScooterById_AddScooterWithIdAndSearchDifferentId_ThrowsInvalidScooterIdExceptionExcetion()
        {
            string searchScooterId = "123";

            _scooterService.AddScooter(DEFAULT_SCOOTER_ID, DEFAULT_PRICE_PER_MINUTE);
           Action action = () => _scooterService.GetScooterById(searchScooterId);

            action.Should().Throw<InvalidScooterIdException>();
        }

        [TestMethod]
        public void GetScooters_AddScootersToTheList_CopyListAndDisplayScooterCount()
        {
            AddThreeScooters();

            IList<Scooter> scooters = _scooterService.GetScooters();

            scooters.Count.Should().Be(3);  
        }

        [TestMethod]
        public void RemoveScooter_AddScootersToList_RemoveScooterByIdAndGetListSize()
        {
            string scooterId = "2";
            AddThreeScooters();

            _scooterService.RemoveScooter(scooterId);

            _scooterStorage.Count.Should().Be(2);   
        }

        [TestMethod]
        public void RemoveScooter_AddScootersToListRemoveOneAndCheckIfIdIsRemoved_ThrowInvalidScooterIdExcetpion()
        {
            string scooterId = "2";
            AddThreeScooters();

            _scooterService.RemoveScooter(scooterId);
            Action action = () => _scooterService.RemoveScooter(scooterId);

            action.Should().Throw<InvalidScooterIdException>();
        }

        [TestMethod]
        public void RemoveScooter_AddScootersToListAndSetWrongScooterId_ThrowInvalidScooterIdException()
        {
            string scooterId = "4";
            AddThreeScooters();

            Action action = () => _scooterService.RemoveScooter(scooterId);

            action.Should().Throw<InvalidScooterIdException>();
        }

        [TestMethod]
        public void RemoveScooter_AddScootersToListAndSetBlankScooterId_ThrowBlankScooterIdException()
        {
            string scooterId = "";
            AddThreeScooters();

            Action action = () => _scooterService.RemoveScooter(scooterId);

            action.Should().Throw<BlankScooterIdException>();
        }

        private void AddThreeScooters()
        {
            _scooterService.AddScooter("1", 0.2m);
            _scooterService.AddScooter("2", 0.4m);
            _scooterService.AddScooter("3", 0.6m);
        }
    }
}