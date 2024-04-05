using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NestQuest.Data.Models
{
    public class Users
    {
        [Key]
        public int User_Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Birthday { get; set; }
        public string UserType { get; set; }
        public bool Two_Fa { get; set; }
        public string Nationality { get; set; }

        [ForeignKey("Guest_Id")]
        public virtual Guest Guest { get; set; }

        [ForeignKey("Host_Id")]
        public virtual Hosts Host { get; set; }
        
        [ForeignKey("Owner_Id")]
        public virtual Properties Properties { get; set; }

        [ForeignKey("Guest_Id")]
        public virtual Favorites Favorites { get; set; }

        [ForeignKey("Guest_Id")]
        public virtual Bookings Bookings { get; set; }
    }
}
