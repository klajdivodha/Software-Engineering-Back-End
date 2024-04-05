using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NestQuest.Data.Models
{
    public class Reportings
    {
        [Key, ForeignKey("Bookings")]
        public int Guest_Id { get; set; }
        [Key, ForeignKey("Bookings")]
        public int Property_Id { get; set; }
        [Key, ForeignKey("Bookings")]
        public DateTime Start_Date { get; set; }
        public string Reporting_User_Type { get; set; }
        public string Status { get; set; }
        public double? Fine {get; set;}
        public string Description { get; set; }

    }
}
