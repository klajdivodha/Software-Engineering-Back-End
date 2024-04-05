using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NestQuest.Data.Models
{
    public class Favorites
    {
        public int Guest_Id { get; set; }
        public int Property_Id { get; set; }
    }
}
