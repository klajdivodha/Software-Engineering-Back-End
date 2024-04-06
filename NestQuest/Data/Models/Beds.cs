using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NestQuest.Data.Models
{
    public class Beds
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }
        public int Number {get; set;}

        [ForeignKey("Properties")]
        public int Property_ID { get; set; }

        public virtual Properties Properties { get; set; }

    }
}
