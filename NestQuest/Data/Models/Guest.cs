using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NestQuest.Data.Models
{
    public class Guest
    {
        // Foreign key and primary key
        [Key, ForeignKey("User")]
        public int Guest_Id { get; set; }
        // Navigation property
        public virtual Users User { get; set; }
        public double rating { get; set; }
        public int Nr_Of_Ratings { get; set; }
        public bool banned { get; set; }
    }
}
    
