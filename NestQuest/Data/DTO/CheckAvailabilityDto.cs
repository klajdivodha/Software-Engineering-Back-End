namespace NestQuest.Data.DTO
{
    public class CheckAvailabilityDto
    {
        public int Property_Id { get; set; } 
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NrOfGuests { get; set; }
    }
}
