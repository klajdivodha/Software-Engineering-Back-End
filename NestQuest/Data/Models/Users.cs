using System.ComponentModel.DataAnnotations;

namespace NestQuest.Data.Models
{
    public class Users
    {
        [Key] public int User_Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Birthday { get; set; }
        public string UserType { get; set; }
        public bool Two_Fa { get; set; }
}
}
