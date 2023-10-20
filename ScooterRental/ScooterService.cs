using ScooterRental.Exceptions;

namespace ScooterRental
{
    public class ScooterService : IScooterService
    {
        private readonly List<Scooter> _scooters;

        public ScooterService(List<Scooter> scooterStorage)
        {
            _scooters = scooterStorage;
        }

        public void AddScooter(string id, decimal pricePerMinute)
        {
            if(_scooters.Any(s => s.Id == id))
            {
                throw new DuplicateScooterExcetpion();
            }

            if(pricePerMinute <= 0)
            {
                throw new NegativeScooterPriceException();
            }

            ValidateScooterId(id);

            _scooters.Add(new Scooter(id, pricePerMinute));
        }

        public Scooter GetScooterById(string scooterId)
        {
            ValidateScooterId(scooterId);

            ValidateScooterExistance(scooterId);

            return _scooters.FirstOrDefault(s => s.Id == scooterId);
        }

        public IList<Scooter> GetScooters()
        {
            return _scooters.ToList();
        }

        public void RemoveScooter(string id)
        {
            ValidateScooterId(id);

            ValidateScooterExistance(id);

            _scooters.RemoveAll(s => s.Id == id);
        }

        private void ValidateScooterId(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new BlankScooterIdException();
            }
        }

        private void ValidateScooterExistance(string id)
        {
            if (!_scooters.Any(s => s.Id == id))
            {
                throw new InvalidScooterIdException();
            }
        }
    }
}
