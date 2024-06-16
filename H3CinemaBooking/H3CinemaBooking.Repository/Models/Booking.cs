using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace H3CinemaBooking.Repository.Models
{
    public class Booking
    {
        [Key]
        public int BookingID { get; set; }
        public int ShowID { get; set; }
        public int UserDetailID { get; set; }
        public int NumberOfSeats { get; set; }
        public double Price { get; set; }
        public bool IsActive { get; set; }
        [JsonIgnore]
        public virtual List<BookingSeat> ?BookingSeats { get; set; }
        [JsonIgnore]
        public Show ?Show { get; set; }
        [JsonIgnore]
        public UserDetail? userDetail { get; set; }

    }
}
