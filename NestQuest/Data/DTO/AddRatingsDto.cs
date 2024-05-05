namespace NestQuest.Data.DTO
{
    public class AddRatingsDto
    {
        public int Property_Id { get; set; }
        public double Cleanliness_Rating { get; set; }
        public double Accuracy_Rating { get; set; }
        public double Checkin_Rating { get; set; }
        public double Communication_Rating { get; set; }
        public double Location_Rating { get; set; }
        public double Price_Rating { get; set; }
    }
}
