namespace NestQuest.Data.DTO
{
    public class AddReportingsDto
    {
        public int Guest_Id { get; set; }
        public int Property_Id { get; set; }
        public DateTime Start_Date { get; set; }
        public DateTime BookingTime { get; set; }
        public string Description { get; set; }
        public IFormFile photo { get; set; }
    }
}
