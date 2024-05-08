namespace NestQuest.Data.DTO
{
    public class AddReportingsDto
    {
        public int Guest_Id { get; set; }
        public int Property_Id { get; set; }
        public DateTime Start_Date { get; set; }
        public DateTime BookingTime { get; set; }
        public string Reporting_User_Type { get; set; }
        public string Status { get; set; }
        public double? Fine { get; set; }
        public string Description { get; set; }
        public IFormFile photo { get; set; }
    }
}
