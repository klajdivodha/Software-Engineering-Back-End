using System.ComponentModel.DataAnnotations;

namespace NestQuest.Data.Models
{
    public class Guest
    {
        [Key] public int Guest_Id { get; set; }
        public double rating { get; set; }
        public bool banned { get; set; }
    }
}
