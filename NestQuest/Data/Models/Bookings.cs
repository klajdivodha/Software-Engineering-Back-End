using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NestQuest.Data.Models
{
    public class Bookings
    {
        public int Guest_Id { get; set;}
        public int Property_Id { get; set;}
        public string Property_Name { get; set;}
        public DateTime BookingTime { get; set; }
        public DateTime Start_Date { get; set;}
        public DateTime End_Date { get; set;}
        public double Amount { get; set;}
        public string Status { get; set;}
    }
}
