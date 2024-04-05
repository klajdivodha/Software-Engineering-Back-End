using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NestQuest.Data.Models
{
    public class Hosts
    {
        [Key, ForeignKey("User")]
        public int Host_Id { get; set; }
        public virtual Users User { get; set; }
        public bool aproved { get; set; }
        public bool banned { get; set; }
        public DateTime startDate { get; set; }
        public double rating { get; set; }

    }
}
