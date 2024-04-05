using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NestQuest.Data.Models
{
    public class Bookings
    {
        [Key, ForeignKey("Users")]
        public int Guest_Id { get; set;}

        [Key, ForeignKey("Properties")]
        public int Property_Id { get; set;}
        [Key]
        public DateTime Start_Date { get; set;}
        public DateTime End_Date { get; set;}
        public double Amount { get; set;}
        public string Status { get; set;}

        [ForeignKey("Property_Id")]
        public virtual Reportings Reportings { get; set; }

        [ForeignKey("Guest_Id")]
        public virtual Reportings Reportings2 { get; set; }

        [ForeignKey("Start_Date")]
        public virtual Reportings Reportings3 { get; set; }
    }
}
