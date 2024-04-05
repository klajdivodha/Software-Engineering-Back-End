using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NestQuest.Data.Models
{
    public class Utilities
    {
        [Key]
        public int Id { get; set; }
        public string Utilitie { get; set; }

        [ForeignKey("Properties")]
        public int Property_Id { get; set; }
    }
}
