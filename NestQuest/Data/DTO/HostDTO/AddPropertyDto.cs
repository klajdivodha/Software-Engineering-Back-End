namespace NestQuest.Data.DTO.HostDTO
{
    public class AddPropertyDto
    {
        public int OwnerId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public double DailyPrice { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime? PremiumFeeStart { get; set; }
        public int NrOfRooms { get; set; }
        public int MaxNrOfGuests { get; set; }
        public bool PetsAllowed { get; set; }
        public int NrOfBaths { get; set; }
        public string CheckinTime { get; set; }
        public string CheckoutTime { get; set; }
        public bool PartiesAllowed { get; set; }
        public bool SmokingAllowed { get; set; }
        public int NrOfParkingSpots { get; set; }
    }
}
