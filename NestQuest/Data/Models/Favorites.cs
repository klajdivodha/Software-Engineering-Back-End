using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NestQuest.Data.Models
{
    public class Favorites
    {
        [Key,ForeignKey("Users")]
        public int Guest_Id { get; set; }

        [Key, ForeignKey("Properties")]
        public int Popertie_Id { get; set; }
    }
}
