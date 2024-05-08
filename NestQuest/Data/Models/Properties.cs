using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NestQuest.Data.Models
{
    public class Properties
    {
        [Key]
        public int Property_ID { get; set; }
        public int Owner_ID { get; set; }
        public bool Availability { get; set; }
        public string Name { get; set;}
        public string Description { get; set;}
        public string Type { get; set;}
        public string Address { get; set;}
        public string City { get; set;}
        public string Country { get; set;}
        public double Daily_Price { get; set;}
        public double Latitude { get; set;}
        public double Longitude { get; set;}
        public DateTime? Preium_Fee_Start { get; set;}
        public int Nr_Rooms { get; set;}
        public int Max_Nr_Of_Guests { get; set;}
        public bool Pets {  get; set;}
        public int Nr_Of_Baths { get; set;}
        public int Nr_Of_Bookings { get; set;}
        public string Checkin_Time { get; set;}
        public string Checkout_Time { get;set;}
        public bool Parties { get; set;}
        public bool Smoking { get; set; }
        public int Nr_Of_Parking_Spots { get; set;}
        public double Cleanliness_Rating { get; set;}
        public double Accuracy_Rating { get; set;}
        public double Checkin_Rating { get;set;}
        public double Communication_Rating { get;set;}
        public double Location_Rating { get;set;}
        public double Price_Rating { get;set;}
        public double Overall_Rating { get; set; }
        public int Nr_Of_Ratings { get; set; }

        [ForeignKey("Property_ID")]
        public virtual ICollection<Beds> Beds { get; set; }

        [ForeignKey("Property_ID")]
        public virtual  ICollection<Utilities> Utilities { get; set; }

        [ForeignKey("Property_ID")]
        public virtual ICollection<Reviews> Reviews { get; set; }
    }
}
