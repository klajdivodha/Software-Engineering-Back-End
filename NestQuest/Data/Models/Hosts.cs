using System.ComponentModel.DataAnnotations;

namespace NestQuest.Data.Models
{
    public class Hosts
    {
        [Key] public int Host_Id { get; set; }
        public bool aproved { get; set; }
        public bool banned { get; set; }
    }
}
