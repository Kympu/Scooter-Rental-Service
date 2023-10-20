namespace ScooterRental
{
    public class RentedScooter
    {
        public string Id { get; }
        public DateTime RentStart { get; }
        public DateTime? RentEnd { get; set; }
        public decimal? TotalRentPrice { get; set; }

        public RentedScooter(string id, DateTime startTime)
        {
            Id = id;
            RentStart = startTime;
        }
    }
}
