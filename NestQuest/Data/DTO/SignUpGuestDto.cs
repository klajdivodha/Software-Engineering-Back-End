namespace NestQuest.Data.DTO
{
    public class SignUpGuestDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Birthday { get; set; }
        public bool Two_Fa { get; set; }
        public string Nationality { get; set; }
    }
}
